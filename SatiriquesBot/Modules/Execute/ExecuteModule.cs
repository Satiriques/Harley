using Microsoft.CodeAnalysis.CSharp.Scripting;
using Discord.Commands;
using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis.Scripting;

public class ExecuteModule : ModuleBase<SocketCommandContext>
{
    [RequireOwner]
    [Command("eval", RunMode = RunMode.Async)]
    public async Task ExecuteAsync([Remainder]string input)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x=>!x.IsDynamic && !string.IsNullOrEmpty(x.Location));
        var globals = new Globals() { Context = Context };
        var options = ScriptOptions.Default
            .WithImports(
                "System.IO", 
                "Discord", 
                "Discord.Commands", 
                "System.Threading.Tasks", 
                "System.Collections.Generic",
                "Newtonsoft.Json")
            .WithReferences(assemblies);
        try
        {
            var result = await ReplyAsync((await CSharpScript.EvaluateAsync(input.Replace("```",""), options, globals)).ToString());
        }
        catch (CompilationErrorException e)
        {
            await ReplyAsync(string.Join(Environment.NewLine, e.Diagnostics));
        }
    }

    [RequireOwner]
    [Command("execute", RunMode = RunMode.Async)]
    public async Task EvalAsync([Remainder]string input)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => !x.IsDynamic && !string.IsNullOrEmpty(x.Location));
        var globals = new Globals() { Context = Context };
        var options = ScriptOptions.Default
            .WithImports(
                "System.IO",
                "Discord",
                "Discord.Commands",
                "System.Threading.Tasks",
                "System.Collections.Generic",
                "Newtonsoft.Json",
                "System.Math")
            .WithReferences(assemblies);
        try
        {
            var result = (await CSharpScript.EvaluateAsync(input.Replace("```", ""), options, globals));
            await ReplyAsync(Format.Code("eval completed."));
        }
        catch (CompilationErrorException e)
        {
            await ReplyAsync(string.Join(Environment.NewLine, e.Diagnostics));
        }
    }

    public class Globals
    {
        public SocketCommandContext Context;
    }
}