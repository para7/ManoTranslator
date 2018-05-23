using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;
using Reactive.Bindings;
using ManoTranslatorCLI;

namespace ManoTranslator
{
    class MainWindowViewModel : BindableBase
    {
        public ReactiveProperty<string> InputText { get; set; } = new ReactiveProperty<string>();

        public ReactiveProperty<string> OutputText { get; set; } = new ReactiveProperty<string>();

        // デリゲートコマンド
        public DelegateCommand EncodeCommand { get; }
        public DelegateCommand DecodeCommand { get; }

        private Translator translator;

        public MainWindowViewModel()
        {
            EncodeCommand = new DelegateCommand(this.ExecuteEncode);

            DecodeCommand = new DelegateCommand(this.ExecuteDecode);
            
            this.InputText = new ReactiveProperty<string>();
            InputText.Value = String.Empty;
            this.OutputText = new ReactiveProperty<string>();
            OutputText.Value = "";

            var x = InputText.Value;

            translator = new Translator();
        }

        public void ExecuteEncode()
        {
            OutputText.Value = translator.Encode(InputText.Value);
        }

        public void ExecuteDecode()
        {
            OutputText.Value = translator.Decode(InputText.Value);
        }
    }
}
