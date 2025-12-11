import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { Plano } from '../../../models/plano.model';
import { BeneficiarioService } from '../../../services/beneficiario.service';
import { PlanoService } from '../../../services/plano.service';

@Component({
  selector: 'app-beneficiario-form',
 standalone: true,
  imports: [
    CommonModule, ReactiveFormsModule, RouterLink,
    NzFormModule, NzInputModule, NzButtonModule, NzCardModule, NzSelectModule, NzDatePickerModule
  ],
  templateUrl: './beneficiario-form.html',
  styleUrl: './beneficiario-form.css',
})

export class BeneficiarioForm implements OnInit {
private fb = inject(FormBuilder);
  private beneficiarioService = inject(BeneficiarioService);
  private planoService = inject(PlanoService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private message = inject(NzMessageService);

  form: FormGroup;
  idEdicao: number | null = null;
  titulo = 'Novo Beneficiário';
  loading = false;
  listaPlanos: Plano[] = [];

  constructor() {
    this.form = this.fb.group({
      nomeCompleto: ['', [Validators.required]],
      cpf: ['', [Validators.required]],
      dataNascimento: [null, [Validators.required]],
      email: ['', [Validators.email]],
      planoId: [null, [Validators.required]]
    });
  }

  ngOnInit(): void {
    this.carregarPlanos();

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.idEdicao = Number(id);
      this.titulo = 'Editar Beneficiário';
      this.carregarBeneficiario(this.idEdicao);
    }
  }

  carregarPlanos() {
    this.planoService.getAll().subscribe(planos => {
      this.listaPlanos = planos;
    });
  }

  carregarBeneficiario(id: number) {
    this.loading = true;
    this.beneficiarioService.getById(id).subscribe({
      next: (dados) => {
        this.form.patchValue(dados);
        this.loading = false;
      },
      error: () => this.loading = false
    });
  }

  salvar() {
    if (this.form.invalid) {
      Object.values(this.form.controls).forEach(control => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
      return;
    }

    this.loading = true;
    const dados = this.form.value;

    const request = this.idEdicao
      ? this.beneficiarioService.update(this.idEdicao, dados)
      : this.beneficiarioService.create(dados);

    request.subscribe({
      next: () => {
        this.message.success('Beneficiário salvo com sucesso!');
        this.router.navigate(['/beneficiarios']);
      },
      error: (erro) => {
        this.message.error(erro.error?.mensagem || 'Erro ao salvar beneficiário');
        this.loading = false;
      }
    });
  }
}
