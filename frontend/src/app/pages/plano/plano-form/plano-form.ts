import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { RouterLink, Router, ActivatedRoute } from '@angular/router';
import { PlanoService } from '../../../services/plano.service';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzMessageService } from 'ng-zorro-antd/message';

@Component({
  selector: 'app-plano-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink, NzFormModule, NzInputModule, NzButtonModule, NzCardModule],
  templateUrl: './plano-form.html',
  styleUrls: ['./plano-form.css'],
})

export class PlanoForm implements OnInit {
  private fb = inject(FormBuilder);
  private planoService = inject(PlanoService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private message = inject(NzMessageService);

  form: FormGroup;
  idEdicao: number | null = null;
  titulo = 'Novo Plano';
  loading = false;

  constructor() {
    this.form = this.fb.group({
      nome: ['', [Validators.required]],
      codigo_registro_ans: ['', [Validators.required]]
    });
  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.idEdicao = Number(id);
      this.titulo = 'Editar Plano';
      this.carregarDados(this.idEdicao);
    }
  }

  carregarDados(id: number) {
    this.loading = true;
    this.planoService.getById(id).subscribe({
      next: (plano) => {
        this.form.patchValue(plano);
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
    const plano = this.form.value;
    const request = this.idEdicao
      ? this.planoService.update(this.idEdicao, plano)
      : this.planoService.create(plano);

    request.subscribe({
      next: () => {
        this.message.success('Plano salvo com sucesso!');
        this.router.navigate(['/planos']);
      },
      error: (erro) => {
        this.message.error(erro.error?.mensagem || 'Erro ao salvar');
        this.loading = false;
      }
    });
  }
}
