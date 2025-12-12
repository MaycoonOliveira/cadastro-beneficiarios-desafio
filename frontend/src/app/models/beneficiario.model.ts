export interface Beneficiario {
  id: number;
  nomeCompleto: string;
  cpf: string;
  email: string;
  dataNascimento: string;
  planoId: number;
  plano?: {
    id: number;
    nome: string;
  };
}

export interface BeneficiarioCriacao {
  nomeCompleto: string;
  cpf: string;
  email: string;
  dataNascimento: string;
  planoId: number;
}
