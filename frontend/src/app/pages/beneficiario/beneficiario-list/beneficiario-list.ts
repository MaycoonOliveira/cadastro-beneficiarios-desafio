import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';
import { NzTableModule } from 'ng-zorro-antd/table';
import { Beneficiario } from '../../../models/beneficiario.model';
import { BeneficiarioService } from '../../../services/beneficiario.service';

@Component({
  selector: 'app-beneficiario-list',
  standalone: true,
  imports: [CommonModule, RouterLink, NzTableModule, NzButtonModule, NzIconModule, NzPopconfirmModule],
  templateUrl: './beneficiario-list.html',
  styleUrl: './beneficiario-list.css',
})
export class BeneficiarioList implements OnInit {
  private beneficiarioService = inject(BeneficiarioService);
  private message = inject(NzMessageService);

  beneficiarios: Beneficiario[] = [];
  loading = true;

  ngOnInit(): void {
    this.carregarDados();
  }

  carregarDados() {
    this.loading = true;
    this.beneficiarioService.getAll().subscribe({
      next: (dados) => {
        this.beneficiarios = dados;
        this.loading = false;
      },
      error: () => {
        this.message.error('Erro ao carregar beneficiários');
        this.loading = false;
      }
    });
  }

  excluir(id: number) {
    this.beneficiarioService.delete(id).subscribe({
      next: () => {
        this.message.success('Beneficiário removido');
        this.carregarDados();
      },
      error: (erro) => this.message.error(erro.error?.mensagem || 'Erro ao remover')
    });
  }
}
