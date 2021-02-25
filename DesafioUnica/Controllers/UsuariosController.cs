using Microsoft.AspNetCore.Mvc;
using DesafioUnica.Models;
using System.Net.Mail;
using Microsoft.AspNetCore.Http;
using System.Net;
using System;
using DesafioUnica.ViewModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DesafioUnica.Data;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Net.Http;

namespace DesafioUnica.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly Context _context;

        public UsuariosController(Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Usuarios.ToListAsync());
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// [GET] Tela de cadastro
        [Route("usuario/cadastro")]
        [HttpGet]
        public IActionResult Cadastro()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioId"))) return RedirectToAction(nameof(Perfil));

            return View();
        }


        /// [GET] Tela de aviso de envio de email 
        [HttpGet]
        public IActionResult ConfirmarEmail()
        {
            if (!String.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioId"))) return RedirectToAction(nameof(Perfil));
            if (TempData["RegistrarUsuario"] == null) return RedirectToAction("Index", "Home");

            ViewBag.Msg = TempData["Erro"];
            ViewBag.Email = TempData["EmailUser"];
            TempData["RegistrarUsuario"] = JsonConvert.SerializeObject(JsonConvert.DeserializeObject<Usuario>(TempData["RegistrarUsuario"].ToString()));
            return View();
        }


        /// [GET] Tela de finalizar cadastro
        [HttpGet]
        public IActionResult FinalizarCadastro(string id)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioId"))) return RedirectToAction(nameof(Perfil));

            if (TempData["token"] != null && id.Equals(TempData["token"].ToString()))
            {
                Usuario registrarUsuario = JsonConvert.DeserializeObject<Usuario>(TempData["RegistrarUsuario"].ToString());
                if (registrarUsuario == null) return RedirectToAction(nameof(Cadastro));

                return View(registrarUsuario);
            }
            return RedirectToAction("Index", "Home");
        }

        [Route("usuario/login")]
        /// <summary>
        /// [GET] Tela de login de usuário
        /// </summary>
        [HttpGet]
        public IActionResult Login()
        {
            if (!String.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioId"))) return RedirectToAction(nameof(Perfil));
            return View();
        }


        /// [GET] Tela de dados do usuário logado
        [HttpGet]
        public IActionResult Perfil()
        {
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioId"))) return RedirectToAction(nameof(Login));
            int id = Int32.Parse(HttpContext.Session.GetString("UsuarioId"));
            Usuario usuario = _context.Usuarios.Find(id);
            return View(usuario);
        }


        /// [GET] Editar usuário
        [HttpGet]
        public async Task<IActionResult> EditarPerfil(int? id)
        {
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioId"))) return RedirectToAction("Index", "Home");
            if (id == null)
            {
                return NotFound();
            }

            //Se tentar usar id de outro usuario redireciona pro usuario logado
            if (id.ToString() != HttpContext.Session.GetString("UsuarioId").ToString())
            {
                int? idFind = Int32.Parse(HttpContext.Session.GetString("UsuarioId").ToString());
                return View(idFind);
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }


        /// [GET] Edição de senha de acesso
        [HttpGet]
        public async Task<IActionResult> EditarSenha(int? id)
        {
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioId"))) return RedirectToAction("Index", "Home");
            if (id == null)
            {
                return NotFound();
            }

            //Se tentar usar id de outro usuario redireciona pro usuario logado
            if (id.ToString() != HttpContext.Session.GetString("UsuarioId").ToString())
            {
                int? idFind = Int32.Parse(HttpContext.Session.GetString("UsuarioId").ToString());
                return View(idFind);
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            EditarSenha editarSenha = new EditarSenha();
            EditarSenha senhaEditarSenha = editarSenha;
            senhaEditarSenha.UsuarioId = usuario.UsuarioId; 
            return View(senhaEditarSenha);
        }

        [Route("usuario/recuperar-senha")]
        //[GET] Recuperar senha
        [HttpGet]
        public IActionResult RecuperarSenha()
        {
            if (!String.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioId"))) return RedirectToAction(nameof(Perfil));
            return View();
        }

        [Route("usuario/recuperar-senha")]
        //[POST] Recuperar senha
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RecuperarSenha(RecuperarSenha forgotPasswordModel)
        {
            if (!EmailUsuarioExiste(forgotPasswordModel.Email)) ModelState.AddModelError("Email", "O e-mail não existe");
            ViewBag.MsgSuccess = null;
            if (ModelState.IsValid)
            {
                ViewBag.MsgSuccess = "Foi enviado um e-mail para você";
                SendEmail(forgotPasswordModel.Email, "Usuário", "Recuperação de senha", "Recupere sua senha", "Utilize o link para recuperar o acesso", "https://localhost:44332/Usuarios/");
            }
            return View(forgotPasswordModel);
        }


        ///[POST] Edição de senha de acesso
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarSenha(int id, EditarSenha editarSenhaModel)
        {
            if (id != editarSenhaModel.UsuarioId)
            {
                return NotFound();
            }

            ViewBag.msg = null;
            if (ModelState.IsValid)
            {
                Cryptography cryptography = new Cryptography(MD5.Create());

                var usuario = _context.Usuarios.Find(id);
                usuario.Senha = cryptography.HashGenerate(editarSenhaModel.Senha);
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Perfil));
                }
                catch (DbUpdateConcurrencyException)
                {
                    ViewBag.msg = "Um erro inesperado ocorreu, tente novamente";
                    if (!UsuarioExiste(usuario.UsuarioId))
                    {
                        return NotFound();
                    }
                }
            }
            return View(editarSenhaModel);
        }


        /// [POST] Editar usuário
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarPerfil(int id, Usuario usuario)
        {
            if (id != usuario.UsuarioId)
            {
                return NotFound();
            }

            ViewBag.msg = null;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Perfil));
                }
                catch (DbUpdateConcurrencyException)
                {
                    ViewBag.msg = "Um erro inesperado ocorreu, tente novamente";
                    if (!UsuarioExiste(usuario.UsuarioId))
                    {
                        return NotFound();
                    }
                }
            }
            return View(usuario);
        }


        /// <summary>
        /// [POST] Tela de cadastro 
        /// </summary>
        [HttpPost]
        [Route("usuario/cadastro")]
        [ValidateAntiForgeryToken]
        public IActionResult Cadastro(Cadastro cadastroModel)
        {
            var usuario = new Usuario();
            Cryptography cryptography = new Cryptography(MD5.Create());

            //Verifica se email existe
            if (EmailUsuarioExiste(cadastroModel.Email)) ModelState.AddModelError("Email", "O e-mail inserido já cadastrado!");

            //Verifica se o telefone existe
            if (TelefoneUsuarioExiste(cadastroModel.Telefone)) ModelState.AddModelError("Telefone", "O telefone inserido já cadastrado!");

            //Verifica se o Cpf existe
            if (CpfUsuarioExiste(cadastroModel.Cpf)) ModelState.AddModelError("CPF", "O Cpf inserido já cadastrado!");
            
            //Verifica se a senha a confirmação de senha são iguais
            if (!cryptography.HashVerify(cadastroModel.ConfirmarSenha, cadastroModel.Senha))
            {
                ModelState.AddModelError("Senha", "As senhas não correspondem.");
            }
            //Verifica força da senha
            else if (usuario.VerifyPasswordStrong(cadastroModel.Senha) < 3)
            {
                ModelState.AddModelError("Senha", "A segurança da senha é baixa, tente outra");
            }

            if (ModelState.IsValid)
            {
                usuario.Nome = cadastroModel.Nome;
                usuario.Telefone = cadastroModel.Telefone;
                usuario.Cpf = cadastroModel.Cpf;
                usuario.Email = cadastroModel.Email;
                usuario.Senha = cryptography.HashGenerate(cadastroModel.Senha);


                //Enviar e-mail pra confirmar email pra cadastro 
                SendEmail(usuario.Email, usuario.Nome, "Confirme seu cadastro", "Verificação de Email", "Você criou uma conta no sistema de login .NET CORE, termine seu registro realizando os passos abaixo.", "https://localhost:44332/Usuarios/FinalizarCadastro?id=");
                TempData["RegistrarUsuario"] = JsonConvert.SerializeObject(usuario);
                TempData["EmailUsuario"] = usuario.Email;
                return RedirectToAction(nameof(ConfirmarEmail));
            }

            TempData["RegistrarUsuario"] = JsonConvert.SerializeObject(usuario);
            return View();
        }


        /// [POST] Tela de finalizar cadastro
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FinalizarCadastro(Models.Usuario usuario)
        {
            ViewBag.Msg = null;
            try
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                TempData["token"] = null;
                StartSessionLogin(usuario);
                return RedirectToAction(nameof(Perfil));
            }
            catch
            {
                ViewBag.Msg = "Um erro ocorreu, tente novamente";
                return View();
            }
        }

        [Route("usuario/login")]
        /// [POST] Tela de login de usuário
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(Login loginModel)
        {
            ViewBag.Erro = null;
            if (ModelState.IsValid)
            {
                if (Login(loginModel.Email, loginModel.Senha))
                {
                    return RedirectToAction(nameof(Perfil));
                }
                ViewBag.Erro = "E-mail ou senha incorretos";
            }
            return View();
        }

        //Verifica se o usuario existe pelo ID
        private bool UsuarioExiste(int id)
        {
            return _context.Usuarios.Any(e => e.UsuarioId == id);
        }

        //Verifica se o CPF já esta cadastrado
        private bool CpfUsuarioExiste(string cpf)
        {
            if (String.IsNullOrEmpty(cpf)) return false;

            Usuario procurarCpf = _context.Usuarios.Where(m => m.Cpf.Equals(cpf)).FirstOrDefault();
            if (procurarCpf != null) return true;
            return false;
        }

        //Verifica se um email já esta cadastrado
        private bool EmailUsuarioExiste(string email)
        {
            if (String.IsNullOrEmpty(email)) return false;

            Usuario procurarEmail = _context.Usuarios.Where(m => m.Email.Equals(email)).FirstOrDefault();
            if (procurarEmail != null) return true;
            return false;
        }


        //Verifica se o telefone já está cadastrado
        private bool TelefoneUsuarioExiste(string telefone)
        {
            if (String.IsNullOrEmpty(telefone)) return false;

            Usuario procurarTelefone = _context.Usuarios.Where(m => m.Telefone.Equals(telefone)).FirstOrDefault();
            if (procurarTelefone != null) return true;
            return false;
        }

        /// Enviar email pro usuário
        /// <param name="email">E-mail pro remetente</param>
        /// <param name="title">Título do e-mail</param>
        /// <param name="msg">Mensagem do e-mail</param>
        /// <param name="link">Link de recuperação do e-mail</param>
        public void SendEmail(string email, string nome, string title, string assunto, string msg, string link)
        {
            try
            {
                string token = Guid.NewGuid().ToString();

                MailMessage m = new MailMessage(new MailAddress("desafiosmtp@gmail.com", title), new MailAddress(email))
                {
                    Subject = assunto,
                    Body = string.Format(
                        @"<div marginwidth=""0"" marginheight=""0"" style=""margin:0;padding:0;height:100%;width:100%;background-color:#f7f7f7"">
                            <center>
                                 <table border=""0"" cellpadding=""0"" cellspacin =""0"" style=""border-collapse:collapse;width:600px;background-color:#ffffff;border:1px solid #d9d9d9;font-family:Helvetica,arial,sans-serif;"">
                                    <tbody>
                                        <tr>
                                            <td align=""center"" valign=""top"" style=""font-family:Helvetica,arial,sans-serif"">
                                                <table align=""center"" border=""0"" cellpaddin=""0"" cellspacing=""0"" width=""100%"" style=""border-collapse:collapse"">
                                                    <tbody>
                                                        <tr>
                                                            <td align = ""center"" style = ""padding-top:20px;padding-bottom:20px"">
                                                                <img src=""https://www.tecnologiaunica.com.br/img/logo.png"" alt =""DesafioUnica"" title=""DesafioUnica"" style=""border:0;height:auto;outline:none;width:auto"">
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style=""line-height:160%;width:100%;min-width:100%"">
                                                <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""border-collapse:collapse;background-color:#ffffff;border-top:1px solid #ffffff;border-bottom:1px solid #ffffff;width:100%;min-width:100%"">
                                                    <tbody>
                                                        <tr>
                                                            <td valign=""top"" style=""color:#ffffff;font-size:20px;font-weight:bold;padding-top:0;padding-right:0;padding-bottom:0;padding-left:0;text-align:left;vertical-align:middle;width:100%;min-width:100%"">
                                                                <table height=""90"" align=""center"" valign=""middle"" width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0"" style=""border-collapse:collapse;width:100%;min-width:100%"">
                                                                    <tbody>
                                                                        <tr>
                                                                            <td width=""100%"" align=""center"" valign=""middle"" height=""90"" style=""background-color:#606062;background-clip:padding-box;font-size:30px;ftext-align:center;padding-left:0px;padding-right:0px;width:100%;vertical-align:middle;min-width:100%"">
                                                                                <span style=""color:#ffffff;font-weight:300"">
                                                                                    {0}
                                                                                </span>
                                                                            </td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style=""line-height:160%;color:#404040;font-size:16px;padding-top:20px;padding-bottom:30px;padding-right:72px;padding-left:72px;background:#ffffff"">
                                                <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""border-collapse:collapse"">
                                                    <tbody>
                                                        <tr>
                                                            <td style=""line-height:100%;padding-bottom:20px;text-align:center"">
                                                                <div style=""display:block;line-height:160%;letter-spacing:normal;margin-top:0px;margin-right:0;margin-bottom:0;margin-left:0;text-align:center;color:#888888;font-size:12px;background-color:#fffcf4;padding:10px;border:1px solid #ffe8ab;border-radius:5px"">
                                                                Esta mensagem foi enviada pelo software de confirmação de email do Desafio Unica de forma automática. Por favor, não responda este e-mail.
                                                                </div>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td style=""line-height:160%;padding-bottom:32px;text-align:center"">
                                                                <h2 style=""display:block;font-style:normal;font-weight:bold;line-height:100%;letter-spacing:normal;margin-top:0;margin-right:0;margin-bottom:0;margin-left:0;text-align:center;color:#404040;font-size:20px"">
                                                                Olá<strong style=""color:#C71E1E!important;font-weight:600!important""> {1}!</strong>
                                                                </h2>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style=""line-height:160%;padding-bottom:32px;text-align:center"">
                                                                <p style=""margin:0"">
                                                                    Esta é a <b>{2}</b> da <b>Unica.</b> {3}
                                                                </p>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style=""line-height:160%;padding-bottom:32px;text-align:center"">
                                                                <div style= ""padding:16px;border:1px solid #AD0000;border-radius:4px;display:block;margin:0 auto;width:90%"">
                                                                    <table border= ""0"" width= ""100%"" cellpadding= ""0"" cellspacing= ""0"" >
                                                                        <tbody>
                                                                            <tr>
                                                                                <td style= ""line-height:160%;padding-bottom:10px;text-align:left"">
                                                                                    <p style= ""margin:0;font-size:14px""> 
                                                                                        Este email é apenas para fins <b>educacionais.</b> Sendo de nenhum valor e vínculo com a empresa <b>Unica Tecnologia</b>. O uso do mesmo não poderá ser usado para outros fins, sendo extremamente <b>restrito!</b><br>
                                                                                    </p>
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style=""line-height:160%;padding-bottom:32px;text-align:center"">
                                                                <p style= ""margin:0"">
                                                                Clique no botão abaixo para acessar sua conta e terminar sua <b>{2}</b>.
                                                                </p>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td style=""padding-bottom:32px;text-align:center"">
                                                                <table height= ""56"" align= ""center"" valign= ""middle"" width= ""90%"" border= ""0"" cellpadding= ""0"" cellspacing= ""0"">
                                                                    <tbody>
                                                                        <tr>
                                                                            <td align=""center"" valign=""middle"" style=""background-color:#C71E1E;border-top-left-radius:4px;border-bottom-left-radius:4px;border-top-right-radius:4px;border-bottom-right-radius:4px;background-clip:padding-box;font-size:16px;width: 100%;font-family:Helvetica,arial,sans-serif;text-align:center;color:#ffffff;font-weight:300;padding-left: 0px;padding-right: 0px;"">
                                                                                <span style=""display:flex;width:100%"">
                                                                                    <a style=""color:#fff;line-height: 56px;text-align:center;align-items:center;justify-content:center;height:100%;width:100%;text-decoration:none;"" href= ""{4}{5}"" target= ""_blank""> {0} </a>
                                                                                </span>
                                                                            </td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                   </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style=""font-family:Helvetica,arial,sans-serif;line-height:160%;background:#ffffff"">
                                                <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""border-collapse:collapse"">
                                                    <tbody>
                                                        <tr>
                                                            <td width=""80%"" style=""line-height:160%;padding-bottom:16px;text-align:center"">
                                                                <hr  style=""width:80%;border:0;border-top:1px solid #ddd"">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style=""line-height:130%;padding-bottom:16px;text-align:center;padding-left:12%;padding-right:12%"">
                                                                <p style=""margin:0;color:#999;font-size:12px"">
                                                                     Este email é apenas para fins <b>educacionais.</b> Sendo de nenhum valor e vínculo com a empresa <b>Unica Tecnologia</b>. O uso do mesmo não poderá ser usado para outros fins, sendo extremamente <b>restrito!</b><br>
                                                                    Criado por <a style=""text-decoration:none;color:#181818;"" href=""samuelnunessergio@gmail.com""><b>Samuel Nunes</b></a>
                                                                </p>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style=""line-height:160%;padding-bottom:16px;text-align:center"">
                                                                <hr>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </center>
                        </div>",
                    title, nome, assunto, msg, link, token)
                };

                TempData["token"] = token;

                m.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("desafiosmtp@gmail.com", "desafiosmtp1234@"),
                    EnableSsl = true
                };
                smtp.Send(m);
            }
            catch
            {
                TempData["Erro"] = "Um erro aconteceu. Tente novamente.";
            }
        }


        //Efetuar o login do usuário
        public bool Login(string email, string senha)
        {
            Cryptography cryptography = new Cryptography(MD5.Create());
            string passwordCript = cryptography.HashGenerate(senha);

            Usuario usuario = _context.Usuarios.Where(p => p.Email.Equals(email) && p.Senha.Equals(passwordCript)).FirstOrDefault();
            if (usuario == null) return false;

            StartSessionLogin(usuario);
            return true;
        }


        //Deslogando o usuário - Remove as sessions existentes
        public void Logout()
        {
            HttpContext.Session.Remove("UsuarioNome");
            HttpContext.Session.Remove("UsuarioEmail");
            HttpContext.Session.Remove("UsuarioId");
            Response.Redirect("https://localhost:44332/");
        }


        //Inicia a session
        private void StartSessionLogin(Usuario usuario)
        {
            HttpContext.Session.SetString("UsuarioNome", usuario.Nome);
            HttpContext.Session.SetString("UsuarioEmail", usuario.Email);
            HttpContext.Session.SetString("UsuarioId", usuario.UsuarioId.ToString());
        }

        
    }
}

