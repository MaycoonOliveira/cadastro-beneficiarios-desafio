import { Component, inject, OnInit } from '@angular/core';
import { Plano } from '../../../models/plano.model';
import { PlanoService } from '../../../services/plano.service';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzMessageService } from 'ng-zorro-antd/message';

@Component({
  selector: 'app-plano-list',
  standalone: true,
  imports: [CommonModule, RouterLink, NzTableModule, NzButtonModule, NzIconModule, NzPopconfirmModule],
  templateUrl: './plano-list.html',
  styleUrls: ['./plano-list.css'],
})

export class PlanoList implements OnInit {
private planoService = inject(PlanoService);
  private message = inject(NzMessageService);

  planos: Plano[] = [];
  loading = true;

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
        this.message.error('Erro ao carregar dados');
        this.loading = false;
      }
    });
  }

  excluir(id: number) {
    this.planoService.delete(id).subscribe({
      next: () => {
        this.message.success('Plano excluído com sucesso');
        this.carregarPlanos();
      },
      error: (erro) => this.message.error(erro.error?.mensagem || 'Erro ao excluir')
    });
  }
}
