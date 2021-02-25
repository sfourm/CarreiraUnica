Instalação do projeto
Na pasta "appsettings.json", altere os dados da DefaultConnection para seu SQL Server Local. Através de lá referencio o banco de dados.
Execute pelo console nugget o comando "update-database", caso ter o migrations - se não tiver, crie o migrate com "add-migration NomeDoMigration" e posteriormente execute update-database.
O site está minimalista e para o funcionamento ideal, tente não alterar as opções do projeto, como HTTP/HTTPS ou versões.
Utilizei .NET Core 5 e Identity Framework (com suas ramificações) 5.0.3 no VS 2019
Aqui está o conograma para auxiliação
Criar model ContaBancaria:
- IdConta: int

- CPFUsuario (recuperar do perfil): string

- NumeroConta: int

- Saldo: int

Associar conta bancária de alguma maneira ao usuario.
Realizar migration no banco de dados - TabelaConta
Criar API XPTO ContaBancaria (Receber/Remover/Editar/Adicionar)
Consumir a api XPTO.
Receber os dados json.
Serializar json para um obj.
Retornar exibir objeto para o cliente (aba minha conta).
Definir as operações via api Transferencia, Deposito e Saque
Transferencia:
- Saldo não pode ficar negativo.

- Transferencia nao pode ser negativa.

- Atualizar saldo pessoal.

- Informar destinatário pelo numero da conta.

- Verificar veracidade da conta dasnatário.

- Atualizar saldo da conta destinatário.

Deposito:
- Informar valor não negativo.

- Informar destinatário pelo numero da conta.

- Atualizar saldo da conta destinatário.

Saque:
- Saldo não pode ficar negativo

- Atualizar saldo pessoal.

O intuito do projeto é desenvolver e entender um pouco sobre as funcionalidades e recursos da linguagem C# ensinados no bootcamp Carreira Única. O projeto consiste em uma aplicação MVC com os recursos:
Cadastro de Usuarios
Autenticação via token (por sessão)
Confirmação via email
Criptografia MD5
Banco de dados SQL Server
Estilização com modais (login/recuperar senha/edições do perfil)
O projeto teve a utilização de alguns recursos disponibilizados na internet, como vídeo-aulas externas e alguns tutoriais, entretanto, a sua criação e proposta veio através de conteúdos ensinados durante o bootcamp oferecido pela Tecnologia Unica.
A construção do mesmo tem

nenhum vínculo

com a empresa, e sua criação foi totalmente com fies educacional (por conta da utilização do nome/identidade visual). Os dados associados a empresa foram todos retirados do site oficial., sem uma permissão direta.
Projeto criado por Samuel Nunes
samuelnunessergio@gmail.com

(35) 99244-2709
