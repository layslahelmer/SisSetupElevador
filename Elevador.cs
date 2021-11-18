using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elevador
{
    public partial class Elevador : Form
    {

        Random _number = new Random();
        private int _maxAndar = 0;
        private int _time = 1500;
        public bool Iniciar = false;


        public ICollection<Acao> Acao { get; set; }


        public Elevador()
        {
            InitializeComponent();

            // Inicializando as configurações dos andares
            Acao = ConfigElevador();
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


                await Task.Delay(TimeSpan.FromMilliseconds(_time));

                Andar _andar = GetAndar(i);

                if (_andar.Id <= _maxAndar)
                {
                    if (_andar.Click)
                    {
                        txtStatus.Text = "<- ->";

                        await Task.Delay(TimeSpan.FromMilliseconds(_time));

                        txtStatus.Text = _andar.Id.ToString();

                        await Task.Delay(TimeSpan.FromMilliseconds(_time));

                        txtStatus.Text = "-> <-";
                    }
                    else
                    {
                        txtStatus.Text = _andar.Id.ToString();
                    }

                    // Reinicia a contagem se o automatico estiver habilitado
                    if (checkBoxAutomatico.Checked && i == 9)
                    {
                        LiberaPainel();
                        Acao = ConfigElevador();
                        setMaxAndar(0);

                        i = 0;
                    }

                }                    
            }

            Iniciar = false;

            LiberaPainel();
            Acao = ConfigElevador();
            setMaxAndar(0);

            return true;            
        }

        public int GetAndar()
        {
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


        public void Simulador()
        {

        }

        // Log de operação do Elevador
        public void Log()
        {

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
        }

        public void setMaxAndar(int i)
        {
            if(_maxAndar < i)
            {
                _maxAndar = i;
            }
        }


        private void painelInterno(int id)
        {
            switch (id)
            {
                case 0:
                    setMaxAndar(0);
                    btnTerreo.BackColor = Color.Blue;
                    Acao.Remove(GetAcao(0));
                    //Acao.Remove(GetAcao(9));
                    Acao.Add(new Acao { Id = 0, Ordem = 0, Andar = new Andar { Id = 0, Click = true } });
                    //Acao.Add(new Acao { Id = 9, Ordem = 1, Andar = new Andar { Id = 0, Click = true } });
                    break;

                case 1:
                    setMaxAndar(1);
                    btn1Andar.BackColor = Color.Blue;
                    Acao.Remove(GetAcao(1));
                    //Acao.Remove(GetAcao(8));
                    Acao.Add(new Acao { Id = 1, Ordem = 0, Andar = new Andar { Id = 1, Click = true } });
                    //Acao.Add(new Acao { Id = 8, Ordem = 1, Andar = new Andar { Id = 1, Click = true } });
                    break;
                case 2:
                    setMaxAndar(2);
                    btn2Andar.BackColor = Color.Blue;
                    Acao.Remove(GetAcao(2));
                    //Acao.Remove(GetAcao(7));
                    Acao.Add(new Acao { Id = 2, Ordem = 0, Andar = new Andar { Id = 2, Click = true } });
                    //Acao.Add(new Acao { Id = 7, Ordem = 1, Andar = new Andar { Id = 2, Click = true } });
                    break;
                case 3:
                    setMaxAndar(3);
                    btn3Andar.BackColor = Color.Blue;
                    Acao.Remove(GetAcao(3));
                    //Acao.Remove(GetAcao(6));
                    Acao.Add(new Acao { Id = 3, Ordem = 0, Andar = new Andar { Id = 3, Click = true } });
                    // Acao.Add(new Acao { Id = 6, Ordem = 1, Andar = new Andar { Id = 3, Click = true } });
                    break;
                case 4:
                    setMaxAndar(4);
                    btn4Andar.BackColor = Color.Blue;
                    Acao.Remove(GetAcao(4));
                    Acao.Add(new Acao { Id = 4, Ordem = 0, Andar = new Andar { Id = 4, Click = true } });
                    break;

            }


        }


        private void painelExterno(int id)
        {
            switch (id)
            {
                case 0:
                    setMaxAndar(0);
                    btnTerreoSobe.BackColor = Color.Blue;
                    Acao.Remove(GetAcao(0));
                    Acao.Add(new Acao { Id = 0, Ordem = 0, Andar = new Andar { Id = 0, Click = true } });
                    break;

                case 1:
                    setMaxAndar(1);
                    btn1AndarSobe.BackColor = Color.Blue;
                    Acao.Remove(GetAcao(1));
                    Acao.Add(new Acao { Id = 1, Ordem = 0, Andar = new Andar { Id = 1, Click = true } });
                    break;
                case 2:
                    setMaxAndar(2);
                    btn2AndarSobe.BackColor = Color.Blue;
                    Acao.Remove(GetAcao(2));
                    Acao.Add(new Acao { Id = 2, Ordem = 0, Andar = new Andar { Id = 2, Click = true } });
                    break;
                case 3:
                    setMaxAndar(3);
                    btn3AndarSobe.BackColor = Color.Blue;
                    Acao.Remove(GetAcao(3));
                    Acao.Add(new Acao { Id = 3, Ordem = 0, Andar = new Andar { Id = 3, Click = true } });
                    break;
                case 4:
                    setMaxAndar(4);
                    btn4AndarDesce.BackColor = Color.Blue;
                    Acao.Remove(GetAcao(4));
                    Acao.Add(new Acao { Id = 4, Ordem = 0, Andar = new Andar { Id = 4, Click = true } });
                    break;
                case 5:
                    setMaxAndar(4);
                    btn4AndarDesce.BackColor = Color.Blue;
                    Acao.Remove(GetAcao(5));
                    Acao.Add(new Acao { Id = 5, Ordem = 1, Andar = new Andar { Id = 4, Click = true } });
                    break;
                case 6:
                    setMaxAndar(3);
                    btn3AndarDesce.BackColor = Color.Blue;
                    Acao.Remove(GetAcao(6));
                    Acao.Add(new Acao { Id = 6, Ordem = 1, Andar = new Andar { Id = 3, Click = true } });
                    break;
                case 7:
                    setMaxAndar(2);
                    btn2AndarDesce.BackColor = Color.Blue;
                    Acao.Remove(GetAcao(7));
                    Acao.Add(new Acao { Id = 7, Ordem = 1, Andar = new Andar { Id = 2, Click = true } });
                    break;
                case 8:
                    setMaxAndar(1);
                    btn1AndarDesce.BackColor = Color.Blue;
                    Acao.Remove(GetAcao(8));
                    Acao.Add(new Acao { Id = 8, Ordem = 1, Andar = new Andar { Id = 1, Click = true } });
                    break;
                case 9:
                    setMaxAndar(0);
                    btnTerreoSobe.BackColor = Color.Blue;
                    Acao.Remove(GetAcao(9));
                    Acao.Add(new Acao { Id = 9, Ordem = 1, Andar = new Andar { Id = 0, Click = true } });
                    break;

            }
        }


        private void btnTerreo_Click(object sender, EventArgs e)
        {
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

            setMaxAndar(3);

            if (Iniciar == false)
            {
                Task<bool> task = IniciarElevador();
            }

            btn3AndarSobe.BackColor = Color.Blue;

            Acao.Remove(GetAcao(3));            
            Acao.Add(new Acao { Id = 3, Ordem = 0, Andar = new Andar { Id = 3, Click = true } });
            

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


        }

        private void txtStatus_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
