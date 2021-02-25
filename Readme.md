<h1>Aqui está o conograma para auxiliação</h1>
<hr/>
<br/>
<h3>Criar model ContaBancaria:</h3>
	<p>     {</p>
    <p>         - IdConta: int</p>
    <p>         - CPFUsuario (recuperar do perfil): string</p>				
    <p>         - NumeroConta: int	</p>
    <p>         - Saldo: int</p>
	<p>     }</p>

<h3> Associar conta bancária de alguma maneira ao usuario.</h3>

<h3> Realizar migration no banco de dados - TabelaConta</h3>

<h3> Criar API XPTO ContaBancaria (Receber/Remover/Editar/Adicionar)</h3>

<h3> Consumir a api XPTO.</h3>

<h3> Receber os dados json.</h3>

<h3> Serializar json para um obj.</h3>

<h3> Retornar exibir objeto para o cliente (aba minha conta).</h3>

<h3> Definir as operações via api Transferencia, Deposito e Saque</h3>
	<h4>Transferencia:</h4>
	<p>     {</p>
	<p>	        - Saldo não pode ficar negativo.</p>
	<p>	        - Transferencia nao pode ser negativa.</p>
	<p>	        - Atualizar saldo pessoal.</p>
	<p>	        - Informar destinatário pelo numero da conta.</p>
	<p>	        - Verificar veracidade da conta dasnatário.</p>
	<p>	        - Atualizar saldo da conta destinatário.</p>
	<p>     }</p>
    <br/>
	<h4>Deposito:</h4>
	<p>     {</p>
	<p>	        - Informar valor não negativo.</p>
	<p>	        - Informar destinatário pelo numero da conta.</p>
	<p>	        - Atualizar saldo da conta destinatário.</p>
	<p>     }</p>
	<br/>
	<h4>Saque: </h4>
	<p>     {</p>
	<p>	        - Saldo não pode ficar negativo</p>
	<p>	        - Atualizar saldo pessoal.</p>
	<p>     }</p>
    <br/>
<hr/>
<h3>O intuito do projeto é desenvolver e entender um pouco sobre as funcionalidades e recursos C# (.NET Core) aprendidos na bootcamp Carreira Única.O projeto consiste em uma aplicação MVC com os recursos:</h3>
<ul>
    <li> Cadastro de Usuarios</li>
    <li> Autenticação via token (por sessão)</li>
    <li> Confirmação via email</li>
    <li> Criptografia MD5</li>
    <li> Banco de dados SQL Server</li>
    <li> Estilização com modais (login/recuperar senha/edições do perfil)</li>
</ul>

<h3>O projeto teve a utilização de alguns recursos disponibilizados na internet, como vídeo-aulas externas e alguns tutoriais, entretanto, a sua criação e proposta veio através de conteúdos ensinados durante o bootcamp oferecido pela Tecnologia Unica. A construção do mesmo não tem nenhum vinculo com a empresa (por conta da utilização do nome/identidade visual). Os dados associados a empresa foram todos retirados do <a href="https://www.tecnologiaunica.com.br/">site oficial.</a></h3>

