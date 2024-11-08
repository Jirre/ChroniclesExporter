﻿using ChroniclesExporter.StateMachine;
using ChroniclesExporter.States;
using Spectre.Console;

namespace ChroniclesExporter;

public static class Program
{
    private static readonly StateMachine<EProgramState> STATE_MACHINE = new();

    private static void Main()
    {
        InitStateMachine();
        STATE_MACHINE.Goto(EProgramState.Init);

        while (!STATE_MACHINE.IsCurrentState(EProgramState.Complete)) STATE_MACHINE.Update();

        DrawCompletionPrompt();
        Console.ReadKey();
    }

    private static void InitStateMachine()
    {
        STATE_MACHINE.Add(new InitState(STATE_MACHINE, EProgramState.Init));
        STATE_MACHINE.Add(new DbLoginState(STATE_MACHINE, EProgramState.DbLogin));
        STATE_MACHINE.Add(new DbTestState(STATE_MACHINE, EProgramState.DbTest));

        STATE_MACHINE.Add(new IndexState(STATE_MACHINE, EProgramState.Index));
        STATE_MACHINE.Add(new MdReadState(STATE_MACHINE, EProgramState.MdRead));
        STATE_MACHINE.Add(new DbWriteState(STATE_MACHINE, EProgramState.DbWrite));
        STATE_MACHINE.Add(new DbLinkState(STATE_MACHINE, EProgramState.DbLink));

        STATE_MACHINE.Add(new LogState(STATE_MACHINE, EProgramState.Log));
        STATE_MACHINE.Add(EProgramState.Complete, CompleteState);
    }

    private static void CompleteState()
    {
    }

    private static void DrawCompletionPrompt()
    {
        Panel panel = new Panel(new Rows(
            new Text("Program Completed", new Style(Color.Green)).Centered(),
            new Text(""),
            new Markup("<Press [blue]Any Key[/] to Close>").Centered())).RoundedBorder();
        panel.Width = 36;
        AnsiConsole.Write(panel);
    }
}
