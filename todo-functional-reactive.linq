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

type Todo = { message: string; completed: bool }

type Filter =
    | All
    | Completed
    | Active

type State = { filter: Filter; todos: Todo list }
let initialState = { filter = All; todos = [] }

type Action =
    | Add of string
    | Toggle of string
    | SetFilter of Filter

let todoApp state message =
    let rec toggleCompletedAt index = function
        | []                                      -> []
        | { message = m; completed = c } as x::xs -> if m = index
                                                     then { x with completed = not c }::toggleCompletedAt index xs
                                                     else x::toggleCompletedAt index xs
    match message with
        | Add message -> { state with todos = state.todos @ [{ message = message; completed = false }] }
        | Toggle index -> { state with todos = toggleCompletedAt index state.todos }
        | SetFilter filter -> { state with filter = filter }

let formatState { filter = f; todos = t } =
    let filter =
        match f with
            | All -> id
            | Completed -> List.filter (fun td -> td.completed)
            | Active -> List.filter (fun td -> not td.completed)
    let format todos =
        let displayStatus = function true -> "" | _ -> " - Incomplete!"
        let displayTodo { message = m; completed = c } = sprintf "* '%s'%s" m (displayStatus c)
        List.map displayTodo todos |> List.reduce (sprintf "%s\n\t%s")
    let display =
        let filterName = function
            | All -> "All"
            | Completed -> "Completed"
            | Active -> "Active"
        sprintf "%s Todos: \n\t%s\n\n" (filterName f)
    t |> filter |> format |> display

let app state message =
    let result = todoApp state message
    printf "%s" (formatState result)
    result

app initialState <| Add "todo 1"
|> app <| Add "todo 2"
|> app <| SetFilter Active
|> app <| Add "todo 3"
|> app <| Toggle "todo 2"
|> app <| SetFilter All |> ignore
