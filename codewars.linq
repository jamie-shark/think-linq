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

let contains x = Seq.filter ((=) x) >> Seq.length >> (<>) 0
let longContains (digit:int64) (long:int64) =
    let longAsString = string long
    let digitAsChar = (string >> char) digit
    contains digitAsChar longAsString

let numbersWithDigitInside (x : int64) (d : int64) =
    let validNumbers = [1L..x] |> Seq.filter (longContains d)
    let count = Seq.length >> int64
    let sum = Seq.fold (+) 0L
    let product = Seq.fold (*) 1L
    
    if Seq.isEmpty validNumbers
        then [0L;0L;0L]
        else [count validNumbers ; sum validNumbers ; product validNumbers]

let Is expected actual =
    match expected = actual with
    | true -> printf "."
    | false -> printfn "F\nExpected %A but got %A" expected actual

contains 2 [1;2;3] |> Is true
contains 4 [1;2;3] |> Is false
contains 'a' "abc" |> Is true
contains 'd' "abc" |> Is false

longContains 1L 111L |> Is true
longContains 2L 111L |> Is false

numbersWithDigitInside 5L 6L |> Is [0L;0L;0L]
numbersWithDigitInside 7L 6L |> Is [1L;6L;6L]
numbersWithDigitInside 11L 1L |> Is [3L;22L;110L]
numbersWithDigitInside 20L 0L |> Is [2L;30L;200L]
numbersWithDigitInside 44L 4L |> Is [9L;286L;5955146588160L]