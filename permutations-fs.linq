<Query Kind="FSharpProgram">
  <Reference>&lt;ProgramFilesX64&gt;\Microsoft SDKs\Azure\.NET SDK\v2.9\bin\plugins\Diagnostics\Newtonsoft.Json.dll</Reference>
  <Reference>C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Linq.Parallel.dll</Reference>
  <Reference>C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Net.dll</Reference>
  <Reference>C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Net.Http.dll</Reference>
  <Reference>C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Net.Http.WebRequest.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Threading.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Threading.Tasks.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Threading.Thread.dll</Reference>
  <Namespace>System.Threading</Namespace>
</Query>

let rec permutations xs n =
    let setWithoutItem set item = Seq.filter (fun i -> Seq.contains item i |> not) set
    let differentItemsOnly seqs = Seq.collect (setWithoutItem seqs)
    if n = 1
        then Seq.map (fun x -> [x]) xs
        else permutations xs (n - 1)
             |> Seq.collect (fun xs' -> differentItemsOnly xs xs')

let permutations' xs' n' =
    let rec helper acc size set =
        seq {
            match size, set with 
            | n, x::xs -> if n > 0  then yield! helper (x::acc) (n - 1) xs
                          if n >= 0 then yield! helper acc n xs 
            | 0, []    -> yield acc 
            | _, []    -> ()
        }
    helper [] n' xs'

let hasDuplicates xs =
    Seq.distinct xs |> Seq.length |> (<>) (Seq.length xs)

let Is expected actual =
    match expected = actual with
    | true -> printf "."
    | false -> printfn "F\nExpected %A but got %A" expected actual

permutations [1;2;3;4;5] 1 |> Seq.length |> Is 5
permutations [1;2;3;4;5] 2 |> Seq.length |> Is 10
permutations [1;2;3;4;5] 3 |> Seq.length |> Is 10
permutations [1;2;3;4;5] 4 |> Seq.length |> Is 5
permutations [1;2;3;4;5] 5 |> Seq.length |> Is 1

permutations [1;2;3;4;5] 3 |> Seq.filter hasDuplicates |> Seq.length |> Is 0

permutations [1;2;3;4;5] 3 |> Seq.map Seq.sort |> Seq.distinct |> Seq.length |> Is 10