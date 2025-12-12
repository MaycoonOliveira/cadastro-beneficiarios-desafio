import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzTableModule } from 'ng-zorro-antd/table';
import { Beneficiario } from '../../../models/beneficiario.model';
import { BeneficiarioService } from '../../../services/beneficiario.service';
import { NzEmptyModule } from 'ng-zorro-antd/empty';
import { FormsModule } from '@angular/forms';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzInputModule } from 'ng-zorro-antd/input';

@Component({
  selector: 'app-beneficiario-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    FormsModule,
    NzTableModule,
    NzButtonModule,
    NzIconModule,
    NzEmptyModule,
    NzModalModule,
    NzSelectModule,
    NzInputModule
  ],
  templateUrl: './beneficiario-list.html',
  styleUrl: './beneficiario-list.css',
})
export class BeneficiarioList implements OnInit {
  private beneficiarioService = inject(BeneficiarioService);
  private message = inject(NzMessageService);

  beneficiariosOriginais: Beneficiario[] = [];
  beneficiarios: Beneficiario[] = [];
  loading = true;
  filtro = '';

  isVisible = false;
  isConfirmLoading = false;
  idParaExcluir: number | null = null;
  prioridadeSelecionada = 3;

  ngOnInit(): void {
    this.carregarDados();
  }

  carregarDados() {
    this.loading = true;
    this.beneficiarioService.getAll().subscribe({
      next: (dados) => {
        this.beneficiariosOriginais = dados;
        this.beneficiarios = dados;
        this.loading = false;
      },
      error: () => {
        this.message.error('Erro ao carregar beneficiários');
        this.loading = false;
      }
    });
  }

  filtrarDados() {
    if (!this.filtro) {
      this.beneficiarios = [...this.beneficiariosOriginais];
    } else {
      const dadosFiltro = this.filtro.toLowerCase();
      this.beneficiarios = this.beneficiariosOriginais.filter(b =>
        b.nomeCompleto.toLowerCase().includes(dadosFiltro) ||
        b.cpf.includes(dadosFiltro)
      );
    }
  }

  abrirModalExclusao(id: number) {
    this.idParaExcluir = id;
    this.prioridadeSelecionada = 3;
    this.isVisible = true;
  }

  cancelarExclusao() {
    this.isVisible = false;
    this.idParaExcluir = null;
  }

  confirmarExclusao() {
    if (this.idParaExcluir === null) return;

    this.isConfirmLoading = true;

    this.beneficiarioService.delete(this.idParaExcluir, this.prioridadeSelecionada).subscribe({
      next: () => {
        this.message.success(`Agendado para exclusão (Prioridade: ${this.prioridadeSelecionada})`);

        this.beneficiariosOriginais = this.beneficiariosOriginais.filter(b => b.id !== this.idParaExcluir);
        this.beneficiarios = this.beneficiarios.filter(b => b.id !== this.idParaExcluir);

        this.isVisible = false;
        this.isConfirmLoading = false;
        this.idParaExcluir = null;
      },
      error: (erro) => {
        this.message.error('Erro ao excluir beneficiário');
        this.isConfirmLoading = false;
      }
    });
  }
}

