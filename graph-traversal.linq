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
let nodeA = { Node = "a"; Connections = [nodeB;nodeE] }

let GetNodes graph =
	let rec helper padding = function
		| []      -> []
		| (x::xs) -> match x with
						| { Node = n; Connections = [] } -> [padding + n] @ helper padding xs
						| { Node = n; Connections = c }  -> [padding + n] @ helper (padding + padding) c @ helper padding xs

	let commaSeparated =
		helper "" [graph]
		|> List.reduce (fun acc x -> sprintf "%s, %s" acc x)

	let tree =
		helper "  " [graph]
		|> List.reduce (fun acc x -> sprintf "%s\r\n%s" acc x)

	tree

GetNodes nodeA
|> printfn "%s"
