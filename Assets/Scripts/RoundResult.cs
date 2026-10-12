using System;
using System.ComponentModel;

public enum RoundResult
{
    Win,
    Draw,
    Lose,
    None
}

public static class RoundResultExtensions
{
    public static string GetDescription(this RoundResult roundResult)
    {
        return roundResult switch
        {
            RoundResult.Win => "Победа!",
            RoundResult.Draw => "Ничья",
            RoundResult.Lose => "Вы проиграли",
            RoundResult.None => string.Empty,
            _ => roundResult.ToString()
        };
    }
}