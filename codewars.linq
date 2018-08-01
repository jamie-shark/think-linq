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

let newavg (values:float seq) avg =
    let total = Seq.reduce (+) values
    let numOfItems = Seq.length values |> (+) 1
    let nextValue = avg * numOfItems - total
    if nextValue <= 0 then None else int nextValue |> Some

let contains x = Seq.filter ((=) x) >> Seq.length >> (<>) 0
let Is expected actual =
    match expected = actual with
    | true -> printf "."
    | false -> printfn "F\nExpected %A but got %A" expected actual

newavg [1.0] 1.0 |> Is (Some 1)
newavg [1.0;3.0] 3.0 |> Is (Some 5)
newavg [3.0] 1.0 |> Is None