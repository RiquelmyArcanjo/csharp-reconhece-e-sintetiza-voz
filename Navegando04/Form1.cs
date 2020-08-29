using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//reconhecimento de fala
using Microsoft.Speech.Recognition;
//síntese de fala (produção artificial de fala humana)
//poderia ser a da Microsoft: using Microsoft.Speech.Synthesis;
using System.Speech.Synthesis;
using System.Globalization;

namespace Navegando04
{
    public partial class Form1 : Form
    {
        static CultureInfo ci = new CultureInfo("pt-BR"); //obj que irá definir linguagem padrão
        static SpeechRecognitionEngine reconhecedor;  //definindo reconhecedor de voz
        SpeechSynthesizer respostaSint = new SpeechSynthesizer(); // sintetizador de voz

        //palavras predefinidas para que o programa entenda, ou seja, não é reconhecimento livre
        public string[] Palavras = { "próximo", "avançar", "seguinte", "anterior", "voltar", "retroceder", "início", "primeiro", "começo", "último", "fim", "final" };
        public Form1()
        {
            InitializeComponent();
            Init();//ao iniciar será chamada
        }
        public void Gramatica()
        {
            try//integrar idioma(pt-BR)
            {
                reconhecedor = new SpeechRecognitionEngine(ci);//instanciando reconhecedor passando como arg o idioma definido
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao integrar idioma escolhido: " + ex.Message);
            }
            var gramatica = new Choices();//choices = palavras q o program vai reconhecer
            gramatica.Add(Palavras);//add a lista de palavras ao obj gramatica que é do tipo Choice

            var gBuilder = new GrammarBuilder(); //um obj grammarBuilder precisa receber um Choice
            gBuilder.Append(gramatica); //inserindo um obj Choice dentro de um obj GrammarBuilder

            try//criando gramática a partir da classe Grammar
            {
                var g = new Grammar(gBuilder);//criando obj Grammar que irá receber o gBuilder como "construtor" 
                try//definições necessárias
                {
                    reconhecedor.RequestRecognizerUpdate();//atualização constante do reconhecedor
                    reconhecedor.LoadGrammarAsync(g);//carrega gramatica, recebendo o nosso obj tipo Grammar
                    reconhecedor.SpeechRecognized += Recognitor;//faz as palavras ditas serem reconhecidas como input do user
                    reconhecedor.SetInputToDefaultAudioDevice();//dispositivo padrão de entrada de acordo com o SO
                    respostaSint.SetOutputToDefaultAudioDevice();//dispositivo padrão de saída de acordo com o SO
                    reconhecedor.RecognizeAsync(RecognizeMode.Multiple);//usará o reconhedor até fechar a aplicação, se for Single usará só uma vez
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao criar reconhecedor: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao criar a gramática: " + ex.Message);
            }
        }
        public void Init()
        {
            respostaSint.Volume = 100;//volume do sintetizador
            respostaSint.Rate = 3;//velocidade da voz do sintetizador
            Gramatica();
        }
        void Recognitor(object sender, SpeechRecognizedEventArgs e)//aqui será comparado o que foi dito
        {
            string entrada = e.Result.Text;
            if (entrada.Equals("próximo") || entrada.Equals("avançar") || entrada.Equals("seguinte"))          
                bindingNavigatorMoveNextItem.PerformClick();     
            
            else if (entrada.Equals("anterior") || entrada.Equals("voltar") || entrada.Equals("retroceder"))
                bindingNavigatorMovePreviousItem.PerformClick();

            else if (entrada.Equals("início") || entrada.Equals("primeiro") || entrada.Equals("começo"))
                bindingNavigatorMoveFirstItem.PerformClick();

            else if(entrada.Equals("último") || entrada.Equals("fim") || entrada.Equals("final"))         
                bindingNavigatorMoveLastItem.PerformClick();

            respostaSint.SpeakAsync("Botão " + entrada + " acionado");     
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: esta linha de código carrega dados na tabela 'bDLivrosDataSet.TabLivro'. Você pode movê-la ou removê-la conforme necessário.
            this.tabLivroTableAdapter.Fill(this.bDLivrosDataSet.TabLivro);
            // TODO: esta linha de código carrega dados na tabela 'bDLivrosDataSet.TabLivro'. Você pode movê-la ou removê-la conforme necessário.
            this.tabLivroTableAdapter.Fill(this.bDLivrosDataSet.TabLivro);
        }
    }
}
