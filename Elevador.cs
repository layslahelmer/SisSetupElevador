using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elevador
{
    public partial class Elevador : Form
    {

        Random _number = new Random();
        readonly string dir = @"c:\temp_log";
        readonly string file = @"c:\temp_log\Elevador_log.txt";
        
        private int _maxAndar = 0;
        private int _time = 1000;
        public bool Iniciar = false;
        

        public ICollection<Acao> Acao { get; set; }

        public Elevador()
        {
            // Se o diretório de log não existe ele é criado                
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            // Se o arquivo de log não existe ele é criado
            if (!File.Exists(file))
            {            
                string createText = "--- Criando o arquivo de log do  Elevador ---" + Environment.NewLine;
                File.WriteAllText(file, createText, Encoding.UTF8);
            }           

            InitializeComponent();
            Log("Iniciando os paineis");


            // Inicializando as configurações dos andares
            Acao = ConfigElevador();
            Log("Iniciando as configurações");
        }
        // Configurações Iniciais do Elevador
        public static ICollection<Acao> ConfigElevador()
        {
            return new List<Acao>
            {
                new Acao {Id = 0, Ordem = 0,Andar = new Andar{Id = 0, Click = false} },
                new Acao {Id = 1, Ordem = 0,Andar = new Andar{Id = 1, Click = false} },
                new Acao {Id = 2, Ordem = 0,Andar = new Andar{Id = 2, Click = false} },
                new Acao {Id = 3, Ordem = 0,Andar = new Andar{Id = 3, Click = false} },
                new Acao {Id = 4, Ordem = 0,Andar = new Andar{Id = 4, Click = false} },
                new Acao {Id = 5, Ordem = 1,Andar = new Andar{Id = 4, Click = false} },
                new Acao {Id = 6, Ordem = 1,Andar = new Andar{Id = 3, Click = false} },
                new Acao {Id = 7, Ordem = 1,Andar = new Andar{Id = 2, Click = false} },
                new Acao {Id = 8, Ordem = 1,Andar = new Andar{Id = 1, Click = false} },
                new Acao {Id = 9,Ordem = 1, Andar = new Andar{Id = 0, Click = false} }
            };             
        }

        public async Task<bool> IniciarElevador()
        {
            Iniciar = true;
            
            for(int i=0; i<10; i++)
            {
                if (checkBoxAutomatico.Checked && i == 0 || checkBoxAutomatico.Checked && i == 2)
                {
                    painelInterno(_number.Next(4));
                    painelExterno(_number.Next(9));
                }               

                Acao _acao = GetAcao(i);

                //Andar _andar = GetAndar(i);

                if (_acao.Andar.Id <= _maxAndar)
                {

                    // Verifica se o Andar atual é o andar clicado pelo usuário
                    if (_acao.Andar.Click)
                    {
                        await AbriPortaAsync();
                        txtStatus.Text = _acao.Andar.Id.ToString();
                        await FechaPortaAsync();
                    }
                    else if (_acao.Andar.Id < _maxAndar)
                    {
                        txtStatus.Text = _acao.Andar.Id.ToString();

                        await Task.Delay(TimeSpan.FromMilliseconds(_time));
                    }

                    await Task.Delay(TimeSpan.FromMilliseconds(_time));

                    // Informando que o elevador está "SUBINDO"
                    if ((_acao.Ordem == 0) && (!_acao.Andar.Click) && (_acao.Id > 0))
                    {
                        Log("Função interna - Elevador Subindo : /\\");
                        txtStatus.Text = "/\\";
                    }

                    // Informando que o elevador está "DESCENDO"
                    if ((_acao.Ordem == 1) && (!_acao.Andar.Click) && (_acao.Id < 9))
                    {
                        Log("Função interna - Elevador Descendo : \\/");
                        txtStatus.Text = "\\/";
                    }
                    await Task.Delay(TimeSpan.FromMilliseconds(_time));

                    // Reinicia a contagem se o automatico estiver habilitado
                    if (checkBoxAutomatico.Checked && i == 9)
                    {
                        Log("Modo de Operação Automático - Reinicia a contagem no Térreo");
                        LiberaPainel();
                        Acao = ConfigElevador();
                        setMaxAndar(0);
                        i = 0;
                    }                         

                }                    
            }

            Log("Modo de Operação Manual - Reinicia a contagem no Térreo");
            Iniciar = false;
            LiberaPainel();
            Acao = ConfigElevador();
            setMaxAndar(0);
            return true;            
        }

        public async Task FechaPortaAsync()
        {
            Log("Função interna - Fechar a porta : -> <-");
            await Task.Delay(TimeSpan.FromMilliseconds(_time));
            txtStatus.Text = "-> <-";
        }


        public async Task AbriPortaAsync()
        {
            Log("Função interna - Abrir a porta : <- ->");
            txtStatus.Text = "<- ->";
            await Task.Delay(TimeSpan.FromMilliseconds(_time));
        }


        public int GetAndar()
        {
            Log("Função Interna - Encontra andar atual");

            ICollection<Acao> Aux = Acao.OrderBy(o => o.Ordem).ToList();
            int i = -1;

            foreach (Acao item in Aux)
            {
                if (item.Andar.Click)
                {
                    i = item.Andar.Id;
                }
            }
            return i;
        }


        // Retonar o objeto andar
        public Andar GetAndar(int Id)
        {

            Log("Função Interna - Encontra andar atual através do id = " + Id.ToString());

            ICollection<Acao> Aux = Acao.OrderBy(o => o.Ordem).ToList();

            foreach (Acao item in Aux)
            {
               if(item.Id == Id)
                {
                    return item.Andar;
                }
            }
            return null;
        }

        public Acao GetAcao(int Id)
        {

            Log("Função Interna - Encontra a Ação atual através do id = " + Id.ToString());

            ICollection<Acao> Aux = Acao.OrderBy(o => o.Ordem).ToList();

            foreach (Acao item in Aux)
            {
                if (item.Id == Id)
                {
                    return item;
                }
            }
            return null;
        }

    
        // Escreve no log de operação do elevador
        public void Log(string text)
        {
            //Se a o text não for nulo ou vazio o conteudo é inserido no arquivo de log
            if (!String.IsNullOrEmpty(text))
            {
                string appendText = DateTime.Now.ToString() + " : " + text + Environment.NewLine;
                File.AppendAllText(file, appendText, Encoding.UTF8);
            }            
        }


        // Método para não permitir há utilização dos dois modos de operação
        public void CheckManual()
        {
            if (checkBoxManual.Checked)
            {
                checkBoxAutomatico.Checked = false;
            }
            else
            {
                checkBoxAutomatico.Checked = true;
            }
        }

        // Método para não permitir há utilização dos dois modos de operação
        public void CheckAutomatico()
        {
            if (checkBoxAutomatico.Checked)
            {
                checkBoxManual.Checked = false;
            }
            else
            {
                checkBoxManual.Checked = true;
            }
        }

        public void LiberaPainel()
        {

            Log("Reiniciando o painel");

            txtStatus.Text = "0";

            btn1Andar.BackColor = Color.White;
            btn2Andar.BackColor = Color.White;
            btn3Andar.BackColor = Color.White;
            btn4Andar.BackColor = Color.White;

            btnTerreoSobe.BackColor = Color.White;
            btn1AndarSobe.BackColor = Color.White;
            btn2AndarSobe.BackColor = Color.White;
            btn3AndarSobe.BackColor = Color.White;

            btnTerreo.BackColor = Color.White;
            btn1AndarDesce.BackColor = Color.White;
            btn2AndarDesce.BackColor = Color.White;
            btn3AndarDesce.BackColor = Color.White;
            btn4AndarDesce.BackColor = Color.White;

            btnEmergencia.BackColor = Color.Red;
        }

        public void setMaxAndar(int i)
        {
            if(_maxAndar < i)
            {
                _maxAndar = i;

                Log("Configurando o Andar máximo em: " + i.ToString());

            }
        }


        private void painelInterno(int id)
        {

            Log("Painel Interno - Automático " + id.ToString());

            switch (id)
            {
                case 0:
                    Log("Painel Interno - Automático  -  btnTerreo");
                    setMaxAndar(0);
                    btnTerreo.BackColor = Color.Blue;
                    Acao.Remove(GetAcao(0));                    
                    Acao.Add(new Acao { Id = 0, Ordem = 0, Andar = new Andar { Id = 0, Click = true } });                    
                    break;

                case 1:
                    Log("Painel Interno - Automático  -  btn1Andar");
                    setMaxAndar(1);
                    btn1Andar.BackColor = Color.Blue;
                    Acao.Remove(GetAcao(1));
                    //Acao.Remove(GetAcao(8));
                    Acao.Add(new Acao { Id = 1, Ordem = 0, Andar = new Andar { Id = 1, Click = true } });
                    //Acao.Add(new Acao { Id = 8, Ordem = 1, Andar = new Andar { Id = 1, Click = true } });
                    break;
                case 2:
                    Log("Painel Interno - Automático  -  btn2Andar");
                    setMaxAndar(2);
                    btn2Andar.BackColor = Color.Blue;
                    Acao.Remove(GetAcao(2));                    
                    Acao.Add(new Acao { Id = 2, Ordem = 0, Andar = new Andar { Id = 2, Click = true } });                    
                    break;
                case 3:
                    Log("Painel Interno - Automático  -  btn3Andar");
                    setMaxAndar(3);
                    btn3Andar.BackColor = Color.Blue;
                    Acao.Remove(GetAcao(3));                    
                    Acao.Add(new Acao { Id = 3, Ordem = 0, Andar = new Andar { Id = 3, Click = true } });                    
                    break;
                case 4:
                    Log("Painel Interno - Automático  -  btn4Andar");
                    setMaxAndar(4);
                    btn4Andar.BackColor = Color.Blue;
                    Acao.Remove(GetAcao(4));
                    Acao.Add(new Acao { Id = 4, Ordem = 0, Andar = new Andar { Id = 4, Click = true } });
                    break;

            }


        }


        private void painelExterno(int id)
        {

            Log("Painel Externo - Automático " + id.ToString());

            switch (id)
            {
                case 0:
                    Log("Painel Externo - Automático - btnTerreoSobe");
                    setMaxAndar(0);
                    btnTerreoSobe.BackColor = Color.Blue;
                    Acao.Remove(GetAcao(0));
                    Acao.Add(new Acao { Id = 0, Ordem = 0, Andar = new Andar { Id = 0, Click = true } });
                    break;

                case 1:
                    Log("Painel Externo - Automático - btn1AndarSobe");
                    setMaxAndar(1);
                    btn1AndarSobe.BackColor = Color.Blue;
                    Acao.Remove(GetAcao(1));
                    Acao.Add(new Acao { Id = 1, Ordem = 0, Andar = new Andar { Id = 1, Click = true } });
                    break;
                case 2:
                    Log("Painel Externo - Automático - btn2AndarSobe");
                    setMaxAndar(2);
                    btn2AndarSobe.BackColor = Color.Blue;
                    Acao.Remove(GetAcao(2));
                    Acao.Add(new Acao { Id = 2, Ordem = 0, Andar = new Andar { Id = 2, Click = true } });
                    break;
                case 3:
                    Log("Painel Externo - Automático - btn3AndarSobe");
                    setMaxAndar(3);
                    btn3AndarSobe.BackColor = Color.Blue;
                    Acao.Remove(GetAcao(3));
                    Acao.Add(new Acao { Id = 3, Ordem = 0, Andar = new Andar { Id = 3, Click = true } });
                    break;
                case 4:
                    Log("Painel Externo - Automático - btn4AndarDesce");
                    setMaxAndar(4);
                    btn4AndarDesce.BackColor = Color.Blue;
                    Acao.Remove(GetAcao(4));
                    Acao.Add(new Acao { Id = 4, Ordem = 0, Andar = new Andar { Id = 4, Click = true } });
                    break;
                case 5:
                    Log("Painel Externo - Automático - btn4AndarDesce");
                    setMaxAndar(4);
                    btn4AndarDesce.BackColor = Color.Blue;
                    Acao.Remove(GetAcao(5));
                    Acao.Add(new Acao { Id = 5, Ordem = 1, Andar = new Andar { Id = 4, Click = true } });
                    break;
                case 6:
                    Log("Painel Externo - Automático - btn3AndarDesce");
                    setMaxAndar(3);
                    btn3AndarDesce.BackColor = Color.Blue;
                    Acao.Remove(GetAcao(6));
                    Acao.Add(new Acao { Id = 6, Ordem = 1, Andar = new Andar { Id = 3, Click = true } });
                    break;
                case 7:
                    Log("Painel Externo - Automático - btn2AndarDesce");
                    setMaxAndar(2);
                    btn2AndarDesce.BackColor = Color.Blue;
                    Acao.Remove(GetAcao(7));
                    Acao.Add(new Acao { Id = 7, Ordem = 1, Andar = new Andar { Id = 2, Click = true } });
                    break;
                case 8:
                    Log("Painel Externo - Automático - btn1AndarDesce");
                    setMaxAndar(1);
                    btn1AndarDesce.BackColor = Color.Blue;
                    Acao.Remove(GetAcao(8));
                    Acao.Add(new Acao { Id = 8, Ordem = 1, Andar = new Andar { Id = 1, Click = true } });
                    break;
                case 9:
                    Log("Painel Externo - Automático - btnTerreoSobe");
                    setMaxAndar(0);
                    btnTerreoSobe.BackColor = Color.Blue;
                    Acao.Remove(GetAcao(9));
                    Acao.Add(new Acao { Id = 9, Ordem = 1, Andar = new Andar { Id = 0, Click = true } });
                    break;

            }
        }


        private void btnTerreo_Click(object sender, EventArgs e)
        {

            Log("Painel Interno - Botão do Térreo - clicado");

            if (!checkBoxAutomatico.Checked)
            {
                setMaxAndar(0);

                if (Iniciar == false)
                {
                    Task<bool> task = IniciarElevador();
                }


                btnTerreo.BackColor = Color.Blue;

                Acao.Remove(GetAcao(0));
                Acao.Remove(GetAcao(9));
                Acao.Add(new Acao { Id = 0, Ordem = 0, Andar = new Andar { Id = 0, Click = true } });
                Acao.Add(new Acao { Id = 9, Ordem = 1, Andar = new Andar { Id = 0, Click = true } });
            }

        }

        private void btn1Andar_Click(object sender, EventArgs e)
        {

            Log("Painel Interno - Botão do 1º Andar - clicado");

            if (!checkBoxAutomatico.Checked)
            {
                setMaxAndar(1);

                if (Iniciar == false)
                {
                    Task<bool> task = IniciarElevador();
                }

                btn1Andar.BackColor = Color.Blue;

                Acao.Remove(GetAcao(1));
                Acao.Remove(GetAcao(8));
                Acao.Add(new Acao { Id = 1, Ordem = 0, Andar = new Andar { Id = 1, Click = true } });
                Acao.Add(new Acao { Id = 8, Ordem = 1, Andar = new Andar { Id = 1, Click = true } });
            }


                
        }

        private void btn3Andar_Click(object sender, EventArgs e)
        {

            Log("Painel Interno - Botão do 3º Andar - clicado");

            if (!checkBoxAutomatico.Checked)
            {
                setMaxAndar(3);

                if (Iniciar == false)
                {
                    Task<bool> task = IniciarElevador();
                }

                btn3Andar.BackColor = Color.Blue;

                Acao.Remove(GetAcao(3));
                Acao.Remove(GetAcao(6));
                Acao.Add(new Acao { Id = 3, Ordem = 0, Andar = new Andar { Id = 3, Click = true } });
                Acao.Add(new Acao { Id = 6, Ordem = 1, Andar = new Andar { Id = 3, Click = true } });
            }
                
        }

        private void btn2Andar_Click(object sender, EventArgs e)
        {

            Log("Painel Interno - Botão do 2º Andar - clicado");

            if (!checkBoxAutomatico.Checked)
            {

                setMaxAndar(2);

                if (Iniciar == false)
                {
                    Task<bool> task = IniciarElevador();
                }

                btn2Andar.BackColor = Color.Blue;

                Acao.Remove(GetAcao(2));
                Acao.Remove(GetAcao(7));
                Acao.Add(new Acao { Id = 2, Ordem = 0, Andar = new Andar { Id = 2, Click = true } });
                Acao.Add(new Acao { Id = 7, Ordem = 1, Andar = new Andar { Id = 2, Click = true } });
            }


                
        }

        private void btn4Andar_Click(object sender, EventArgs e)
        {

            Log("Painel Interno - Botão do 4º Andar - clicado");

            if (!checkBoxAutomatico.Checked)
            {
                setMaxAndar(4);

                if (Iniciar == false)
                {
                    Task<bool> task = IniciarElevador();
                }

                btn4Andar.BackColor = Color.Blue;

                Acao.Remove(GetAcao(4));
                Acao.Remove(GetAcao(9));
                Acao.Add(new Acao { Id = 4, Ordem = 0, Andar = new Andar { Id = 4, Click = true } });
                Acao.Add(new Acao { Id = 9, Ordem = 1, Andar = new Andar { Id = 0, Click = true } });
            }

                

        }

        private void btnEmergencia_Click(object sender, EventArgs e)
        {

            Log("Painel Interno - Botão de emegência - clicado");

            if (!checkBoxAutomatico.Checked)
            {
                if (Iniciar == false)
                {
                    painelInterno(0);
                    Task<bool> task = IniciarElevador();
                    LiberaPainel();
                    Acao = ConfigElevador();
                    setMaxAndar(0);
                }
                else
                {
                    painelInterno(0);
                    LiberaPainel();
                    Acao = ConfigElevador();
                    setMaxAndar(0);
                }

                btnEmergencia.BackColor = Color.Blue;
            }
                
        }

        private void btnTerreoSobe_Click(object sender, EventArgs e)
        {

            Log("Painel Externo - Botão do Térreo Sobe - clicado");

            setMaxAndar(0);

            if (Iniciar == false)
            {
                Task<bool> task = IniciarElevador();
            }


            btnTerreoSobe.BackColor = Color.Blue;

            Acao.Remove(GetAcao(0));            
            Acao.Add(new Acao { Id = 0, Ordem = 0, Andar = new Andar { Id = 0, Click = true } });            

        }

        private void btn1AndarSobe_Click(object sender, EventArgs e)
        {

            Log("Painel Externo - Botão do 1º Andar Sobe - clicado");

            setMaxAndar(1);

            if (Iniciar == false)
            {
                Task<bool> task = IniciarElevador();
            }


            btn1AndarSobe.BackColor = Color.Blue;

            Acao.Remove(GetAcao(1));            
            Acao.Add(new Acao { Id = 1, Ordem = 0, Andar = new Andar { Id = 1, Click = true } });
        }

        private void btn1AndarDesce_Click(object sender, EventArgs e)
        {

            Log("Painel Externo - Botão do 1º Andar Desce - clicado");

            setMaxAndar(1);

            if (Iniciar == false)
            {
                Task<bool> task = IniciarElevador();
            }


            btn1AndarDesce.BackColor = Color.Blue;
                       
            Acao.Remove(GetAcao(8));
            Acao.Add(new Acao { Id = 8, Ordem = 1, Andar = new Andar { Id = 1, Click = true } });
        }

        private void btn2AndarSobe_Click(object sender, EventArgs e)
        {

            Log("Painel Externo - Botão do 2º Andar Sobe - clicado");

            setMaxAndar(2);

            if (Iniciar == false)
            {
                Task<bool> task = IniciarElevador();
            }


            btn2AndarSobe.BackColor = Color.Blue;

            Acao.Remove(GetAcao(2));            
            Acao.Add(new Acao { Id = 2, Ordem = 0, Andar = new Andar { Id = 2, Click = true } });
        }

        private void btn2AndarDesce_Click(object sender, EventArgs e)
        {

            Log("Painel Externo - Botão do 2º Andar Desce - clicado");

            setMaxAndar(2);

            if (Iniciar == false)
            {
                Task<bool> task = IniciarElevador();
            }


            btn2AndarDesce.BackColor = Color.Blue;
                        
            Acao.Remove(GetAcao(7));            
            Acao.Add(new Acao { Id = 7, Ordem = 1, Andar = new Andar { Id = 2, Click = true } });

        }

        private void btn4AndarDesce_Click(object sender, EventArgs e)
        {

            Log("Painel Externo - Botão do 4º Andar Desce - clicado");

            setMaxAndar(4);

            if (Iniciar == false)
            {
                Task<bool> task = IniciarElevador();
            }


            btn4AndarDesce.BackColor = Color.Blue;


            Acao.Remove(GetAcao(4));
            Acao.Add(new Acao { Id = 4, Ordem = 0, Andar = new Andar { Id = 4, Click = true } });

        }

        private void btn3AndarDesce_Click(object sender, EventArgs e)
        {

            Log("Painel Externo - Botão do 3º Andar Desce - clicado");

            setMaxAndar(3);

            if (Iniciar == false)
            {
                Task<bool> task = IniciarElevador();
            }

            btn3AndarDesce.BackColor = Color.Blue;
            
            Acao.Remove(GetAcao(6));
            Acao.Add(new Acao { Id = 6, Ordem = 1, Andar = new Andar { Id = 3, Click = true } });
        }

        private void btn3AndarSobe_Click(object sender, EventArgs e)
        {
            btn3AndarSobe.BackColor = Color.Blue;
        }

        private void btn3AndarSobe_Click_1(object sender, EventArgs e)
        {

            Log("Painel Externo - Botão do 3º Andar Sobe - clicado");

            setMaxAndar(3);

            if (Iniciar == false)
            {
                Task<bool> task = IniciarElevador();
            }

            btn3AndarSobe.BackColor = Color.Blue;

            Acao.Remove(GetAcao(3));            
            Acao.Add(new Acao { Id = 3, Ordem = 0, Andar = new Andar { Id = 3, Click = true } });
            
            Log("Habilitando modo de operação manual");


        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void checkBoxManual_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void checkBoxManual_CheckedChanged(object sender, EventArgs e)
        {
            CheckManual();

            

        }

        private void checkBoxAutomatico_CheckedChanged(object sender, EventArgs e)
        {
            CheckAutomatico();

            if (Iniciar == false)
            {
                Task<bool> task = IniciarElevador();
            }

            Log("Habilitando modo de operação automático");


        }

        private void txtStatus_TextChanged(object sender, EventArgs e)
        {

        }

        private void Elevador_Load(object sender, EventArgs e)
        {
            Log("Iniciando a operação");
        }

        private void Elevador_FormClosed(object sender, FormClosedEventArgs e)
        {
            Log("Finalizando a operação");
        }
    }
}
