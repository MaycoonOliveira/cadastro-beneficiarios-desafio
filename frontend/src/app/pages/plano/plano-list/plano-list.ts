import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzEmptyModule } from 'ng-zorro-antd/empty';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { Plano } from '../../../models/plano.model';
import { PlanoService } from '../../../services/plano.service';

@Component({
  selector: 'app-plano-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    NzTableModule,
    NzButtonModule,
    NzIconModule,
    NzEmptyModule,
    NzModalModule
  ],
  templateUrl: './plano-list.html',
  styleUrls: ['./plano-list.css'],
})

export class PlanoList implements OnInit {
  private planoService = inject(PlanoService);
  private message = inject(NzMessageService);

  planos: Plano[] = [];
  loading = true;

  isVisible = false;
  isConfirmLoading = false;
  idParaExcluir: number | null = null;

  ngOnInit(): void {
    this.carregarPlanos();
  }

  carregarPlanos() {
    this.loading = true;
    this.planoService.getAll().subscribe({
      next: (dados) => {
        this.planos = dados;
        this.loading = false;
      },
      error: () => {
        this.message.error('Erro ao carregar planos');
        this.loading = false;
      }
    });
  }

  abrirModalExclusao(id: number) {
    this.idParaExcluir = id;
    this.isVisible = true;
  }

  cancelarExclusao() {
    this.isVisible = false;
    this.idParaExcluir = null;
  }

  confirmarExclusao() {
    if (this.idParaExcluir === null) return;

    this.isConfirmLoading = true;

    this.planoService.delete(this.idParaExcluir).subscribe({
      next: () => {
        this.message.success('Plano excluído com sucesso');
        this.planos = this.planos.filter(p => p.id !== this.idParaExcluir);

        this.isVisible = false;
        this.isConfirmLoading = false;
        this.idParaExcluir = null;
      },
      error: (erro) => {
        const msg = erro.error?.mensagem || 'Erro ao excluir plano.';
        this.message.error(msg);
        this.isConfirmLoading = false;
      }
    });
  }
}
