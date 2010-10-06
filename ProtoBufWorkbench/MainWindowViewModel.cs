using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using ProtoBufWorkbench.Framework;
using ProtoBufWorkbench.Properties;

namespace ProtoBufWorkbench
{
    public class MainWindowViewModel : ViewModel
    {
        private string _messageDefinition;
        private string _messageText;
        private string _errors;
        private byte[] _messageBinary;
        private string _csharpBinary;
        private ICommand _encodeCommand;
        private ProtoCompiler _compiler;
        private string _rootMessageName;
        private ActionInvokingCommand _decodeCommand;
        private ObservableCollection<string> _availableMessageTypes = new ObservableCollection<string>();

        public MainWindowViewModel()
        {
            _compiler = new ProtoCompiler()
                            {
                                ProtoCompilerPath = Path.Combine(
                                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                                    Settings.Default.ProtoCompiler)
                                    };

            MessageDefinition = Settings.Default.LastMessageDefinition;
        }

        public string MessageDefinition
        {
            get { return _messageDefinition; }
            set
            {
                _messageDefinition = value;
                UpdateRootMessageTypes();
                SaveMessageDefinitionInUserSettings(value);
                OnPropertyChanged(this.GetPropertySymbol(vm => vm.MessageDefinition));
            }
        }

        private void SaveMessageDefinitionInUserSettings(string value)
        {
            Settings.Default.LastMessageDefinition = value;
            Settings.Default.Save();
        }

        private void UpdateRootMessageTypes()
        {
            const string MessageRegex = @"message\s+(\w+)\s+{";

            var originalRootMessage = RootMessageName;

            var availableMessageTypes = Regex.Matches(MessageDefinition, MessageRegex)
                .Cast<Match>()
                .Select(m => m.Groups[1].Value)
                .ToList();

            AvailableMessageTypes.Clear();

            foreach (var availableMessageType in availableMessageTypes)
            {
                AvailableMessageTypes.Add(availableMessageType);
            }

            RootMessageName = string.IsNullOrWhiteSpace(originalRootMessage)
                                  ? AvailableMessageTypes.LastOrDefault()
                                  : AvailableMessageTypes.Contains(originalRootMessage)
                                        ? originalRootMessage
                                        : AvailableMessageTypes.LastOrDefault();
        }

        public string MessageText
        {
            get { return _messageText; }
            set
            {
                _messageText = value;
                OnPropertyChanged(this.GetPropertySymbol(vm => vm.MessageText));
            }
        }

        public string RootMessageName
        {
            get { return _rootMessageName; }
            set
            {
                _rootMessageName = value;
                OnPropertyChanged(this.GetPropertySymbol(vm => vm.RootMessageName));
            }
        }

        public byte[] MessageBinary
        {
            get { return _messageBinary; }
            set
            {
                _messageBinary = value;
                UpdateCSharpBinary();
                OnPropertyChanged(this.GetPropertySymbol(vm => vm.MessageBinary));
            } 
        }

        public string MessageBinaryCSharp
        {
            get { return _csharpBinary; }
            private set
            {
                _csharpBinary = value;
                OnPropertyChanged(this.GetPropertySymbol(vm => vm.MessageBinaryCSharp));
            }
        }

        private void UpdateCSharpBinary()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("new byte[] { ");

            foreach (var messageByte in _messageBinary)
            {
                stringBuilder.AppendFormat("0x{0:x}, ", messageByte);
            }

            stringBuilder.Append("}");
            MessageBinaryCSharp = stringBuilder.ToString();
        }

        public string Errors
        {
            get { return _errors; }
            private set
            {
                _errors = value;
                OnPropertyChanged(this.GetPropertySymbol(vm => vm.Errors));
            }
        }

        public ObservableCollection<string> AvailableMessageTypes
        {
            get { return _availableMessageTypes; }
        }

        public ICommand Encode
        {
            get
            {
                if (_encodeCommand == null)
                {
                    _encodeCommand = new ActionInvokingCommand(DoEncodeMessage);
                }
                return _encodeCommand;
            }
        }

        public ICommand Decode
        {
            get
            {
                if (_decodeCommand == null)
                {
                    _decodeCommand = new ActionInvokingCommand(DoDecodeMessage);
                }
                return _decodeCommand;
            }
        }

        private void DoEncodeMessage()
        {
            var task = new EncodeTask()
                           {
                               MessageDefinition = MessageDefinition,
                               MessageText = MessageText,
                               RootMessageName = RootMessageName,
                           };

            var result = _compiler.Encode(task);
            MessageBinary = result.EncodedMessage;
            Errors = result.CompilerOutput;
        }

        private void DoDecodeMessage()
        {
            var task = new DecodeTask()
            {
                MessageDefinition = MessageDefinition,
                MessageBinary = MessageBinary,
                RootMessageName = RootMessageName,
            };

            var result = _compiler.Decode(task);
            MessageText = result.DecodedMessage;
            Errors = result.CompilerOutput;
        }
    }
}
