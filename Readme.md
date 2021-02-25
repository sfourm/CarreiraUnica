<h3>Instalação do projeto</h3>
<ul>
    <li> Na pasta "appsettings.json", altere os dados da DefaultConnection para seu SQL Server Local. Através de lá referencio o banco de dados. </li>
    <li> Execute pelo console nugget o comando "update-database", caso ter o migrations - se não tiver, crie o migrate com "add-migration NomeDoMigration" e posteriormente execute update-database.</li>
    <li> O site está minimalista e para o funcionamento ideal, tente não alterar as opções do projeto, como HTTP/HTTPS ou versões.</li>
    <li> Utilizei .NET Core 5 e Identity Framework (com suas ramificações) 5.0.3 no VS 2019 </li>
</ul>
<hr/>
<h3>Aqui está o conograma para auxiliação</h3>
<ul>
    <li>Criar model ContaBancaria:
        <p>- IdConta: int</p>
        <p>- CPFUsuario (recuperar do perfil): string</p>				
        <p>- NumeroConta: int	</p>
        <p>- Saldo: int</p>
    </li>
    <li> Associar conta bancária de alguma maneira ao usuario.</li>
    <li> Realizar migration no banco de dados - TabelaConta</li>
    <li> Criar API XPTO ContaBancaria (Receber/Remover/Editar/Adicionar)</li>
    <li> Consumir a api XPTO.</li>
    <li> Receber os dados json.</li>
    <li> Serializar json para um obj.</li>
    <li> Retornar exibir objeto para o cliente (aba minha conta).</li>
    <li> Definir as operações via api Transferencia, Deposito e Saque
        <h4><b>Transferencia:</b></h4>
        <p>- Saldo não pode ficar negativo.</p>
        <p>- Transferencia nao pode ser negativa.</p>
        <p>- Atualizar saldo pessoal.</p>
        <p>- Informar destinatário pelo numero da conta.</p>
        <p>- Verificar veracidade da conta dasnatário.</p>
        <p>- Atualizar saldo da conta destinatário.</p>
        <h4><b>Deposito:</b></h4>
        <p>- Informar valor não negativo.</p>
        <p>- Informar destinatário pelo numero da conta.</p>
        <p>- Atualizar saldo da conta destinatário.</p>
        <h4><b>Saque: </b></h4>
        <p>- Saldo não pode ficar negativo</p>
        <p>- Atualizar saldo pessoal.</p>
    </li>
</ul>  
<hr/>

<h4>O intuito do projeto é desenvolver e entender um pouco sobre as funcionalidades e recursos da linguagem C# ensinados no bootcamp Carreira Única. O projeto consiste em uma aplicação MVC com os recursos:</h4>
<ul>
    <li> Cadastro de Usuarios
    <li> Autenticação via token (por sessão)
    <li> Confirmação via email
    <li> Criptografia MD5
    <li> Banco de dados SQL Server
    <li> Estilização com modais (login/recuperar senha/edições do perfil)
</ul>

<h4>O projeto teve a utilização de alguns recursos disponibilizados na internet, como vídeo-aulas externas e alguns tutoriais, entretanto, a sua criação e proposta veio através de conteúdos ensinados durante o bootcamp oferecido pela Tecnologia Unica.<h4>

<p>A construção do mesmo tem <b>nenhum vínculo</b> com a empresa, e sua criação foi totalmente com viés educacional (por conta da utilização do nome/identidade visual). Os dados associados a empresa foram todos retirados do <a href="https://www.tecnologiaunica.com.br/">site oficial</a>, sem nenhuma permissão direta.</p>

<h3>Projeto criado por Samuel Nunes</h3>
<p>samuelnunessergio@gmail.com</p>
<p>(35) 99244-2709</p>


