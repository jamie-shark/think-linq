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

type Graph<'T> = {
    Node: 'T
    Connections: Graph<'T> list
}

let nodeE = { Node = "e"; Connections = [] }
let nodeD = { Node = "d"; Connections = [] }
let nodeC = { Node = "c"; Connections = [nodeD;nodeE] }
let nodeB = { Node = "b"; Connections = [nodeC] }
let nodeA = { Node = "a"; Connections = [nodeB;nodeC;nodeD;nodeE] }

let isElementOf xs x = xs |> List.filter ((=) x) |> (<>) []

let GetNodes graph =
    let flattenNodes =
        let rec helper depth processed = function
            | []      -> []
            | (x::xs) -> if isElementOf processed x
                         then helper depth processed xs
                         else match x with
                                | { Node = n; Connections = [] } -> [(n, depth)] @ helper depth (x::processed) xs
                                | { Node = n; Connections = c }  -> [(n, depth)] @ helper (depth + 1) (x::processed) c @ helper depth (x::processed) xs

        helper 0 [] [graph]

    let commaSeparated =
        flattenNodes
        |> List.map (fun (value, _) -> value)
        |> List.reduce (fun acc x -> sprintf "%s, %s" acc x)

    let tree =
        let rec pad = function
            | 0 -> ""
            | depth -> "\t" + pad (depth - 1)

        flattenNodes
        |> List.map (fun (value, depth) -> sprintf "%s%s" (pad depth) value)
        |> List.reduce (fun acc x -> sprintf "%s\r\n%s" acc x)

    tree

GetNodes nodeA
|> printfn "%s"