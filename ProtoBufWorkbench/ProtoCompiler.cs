using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using ProtoBufWorkbench.Properties;

namespace ProtoBufWorkbench
{
    public class ProtoCompiler
    {
        public DecodeResult Decode(DecodeTask task)
        {
            var tempDirectory = CreateTempDirectory();
            var arguments = CreateArgumentsForTask(task, tempDirectory);

            try
            {
                using (var protoCProcess = new Process
                                               {
                                                   StartInfo = GetProcessStartInfo(tempDirectory, arguments)
                                               })
                {
                    protoCProcess.Start();

                    var standardInput = protoCProcess.StandardInput;
                    WriteEncodedMessageToStandardInput(standardInput, task.MessageBinary);

                    var output = protoCProcess.StandardOutput.ReadToEnd();
                    var error = protoCProcess.StandardError.ReadToEnd();

                    return new DecodeResult()
                               {
                                   DecodedMessage = output, 
                                   CompilerOutput = error
                               };
                }
            }
            finally
            {
                Directory.Delete(tempDirectory, true);
            }
        }

        public EncodeResult Encode(EncodeTask task)
        {
            var tempDirectory = CreateTempDirectory();
            var arguments = CreateArgumentsForTask(task, tempDirectory);

            try
            {
                using (var protoCProcess = new Process
                {
                    StartInfo = GetProcessStartInfo(tempDirectory, arguments)
                })
                {
                    protoCProcess.Start();

                    var standardInput = protoCProcess.StandardInput;
                    WriteMessageToStandardInput(standardInput, task.MessageText);
                    standardInput.Close();

                    var output = protoCProcess.StandardOutput.ReadToEnd();
                    var error = protoCProcess.StandardError.ReadToEnd();

                    return new EncodeResult()
                    {
                        EncodedMessage = protoCProcess.StandardOutput.CurrentEncoding.GetBytes(output),
                        CompilerOutput = error
                    };
                }
            }
            finally
            {
                Directory.Delete(tempDirectory, true);
            }
        }

        private void WriteMessageToStandardInput(StreamWriter standardInput, string messageText)
        {
            standardInput.Write(messageText);
        }

        private string CreateArgumentsForTask(EncodeTask task, string tempDirectory)
        {
            var definitionFile = WriteDefinitionToFile(tempDirectory, task);

            var arguments = string.Format("--encode {0} --error_format=msvs {1}", task.RootMessageName, definitionFile);
            return arguments;
        }

        private void WriteEncodedMessageToStandardInput(StreamWriter standardInput, byte[] messageBinary)
        {
            using (var memoryStream = new MemoryStream(messageBinary))
            using (var streamReader = new StreamReader(memoryStream, standardInput.Encoding))
            {
                standardInput.Write(streamReader.ReadToEnd());
            }
            
            standardInput.Close();
        }

        private string CreateArgumentsForTask(DecodeTask task, string tempDirectory)
        {
            var definitionfilePath = WriteDefinitionToFile(tempDirectory, task);

            var arguments = string.Format("--decode {0} --error_format=msvs  {1}", task.RootMessageName, definitionfilePath);
            return arguments;
        }

        private string WriteDefinitionToFile(string tempDirectory, ProtoCTask task)
        {
            var definitionfileName = "MessageDefinition.proto";
            var definitionfilePath = Path.Combine(tempDirectory, definitionfileName);

            File.WriteAllText(definitionfilePath, task.MessageDefinition);
            return definitionfileName;
        }

        private ProcessStartInfo GetProcessStartInfo(string tempDirectory, string arguments)
        {
            return new ProcessStartInfo()
                       {
                           CreateNoWindow = true,
                           FileName = ProtoCompilerPath,
                           WorkingDirectory = tempDirectory,
                           UseShellExecute = false,
                           RedirectStandardInput = true,
                           RedirectStandardOutput = true,
                           RedirectStandardError = true,
                           Arguments = arguments,
                       };
        }

        public string ProtoCompilerPath { get; set; }

        public string CreateTempDirectory()
        {
            var temp = Path.GetTempPath();
            var protoCTempPath = Path.Combine(temp, "ProtoBufWorkbench");
            var sessionTempPath = Path.Combine(protoCTempPath, Guid.NewGuid().ToString());

            Directory.CreateDirectory(sessionTempPath);

            return sessionTempPath;
        }
    }
}
