Module Program
    Sub Main()
        Dim tickers = {"MSFT", "ORCL", "EBAY", "GOOG", "AAPL", "C"}
        Const days = 365

        Console.WriteLine("Non parallel:")

        For Each t In tickers
            Dim a = Library.StockAnalyzer.GetAnalyzer(t, days)
            Console.WriteLine($"{t}: Standard Deviation is: {a.StdDev:F4} Return is: {a.Return:F4}")
        Next

        Console.WriteLine()
        Console.ReadLine()

        Console.WriteLine("Parallel:")

        Dim op = Library.StockAnalyzer.GetAnalyzersParalAsync(tickers, days)

        Dim analyzers =
                Microsoft.FSharp.Control.FSharpAsync.RunSynchronously(
                    op,
                    Microsoft.FSharp.Core.FSharpOption(Of Integer).None,
                    Microsoft.FSharp.Core.FSharpOption(Of Threading.CancellationToken).None
                    )

        For i = 0 To analyzers.Length - 1
            Dim a = analyzers(i)
            Console.WriteLine($"{tickers(i)}: Standard Deviation is: {a.StdDev:F4} Return is: {a.Return:F4}")
        Next
    End Sub

End Module
