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

type Action =
    | Add of string
    | Toggle of string
    | SetFilter of Filter

type Store<'State, 'Payload> =
    { getState : unit -> 'State
      dispatch : 'Payload -> 'Payload
      subscribe : (unit -> unit) -> unit -> unit }

let rec toggleCompletedAt index = function
    | []                                      -> []
    | { message = m; completed = c } as x::xs -> if m = index
                                                 then { x with completed = not c }::toggleCompletedAt index xs
                                                 else x::toggleCompletedAt index xs

let filterReducer filter = function
    | SetFilter f -> f
    | _           -> filter

let todosReducer todos = function
    | Add m        -> todos @ [{ message = m; completed = false }]
    | Toggle index -> toggleCompletedAt index todos
    | _            -> todos

let todoApp state message =
    {
      filter = filterReducer state.filter message
      todos  = todosReducer state.todos message
    }

let formatState { filter = f; todos = t } =
    let filter =
        match f with
            | All       -> id
            | Completed -> List.filter (fun td -> td.completed)
            | Active    -> List.filter (fun td -> not td.completed)
    let format todos =
        let displayStatus = function true -> "" | _ -> " - Incomplete!"
        let displayTodo { message = m; completed = c } = sprintf "* '%s'%s" m (displayStatus c)
        List.map displayTodo todos |> List.reduce (sprintf "%s\n\t%s")
    let display =
        let filterName = function
            | All       -> "All"
            | Completed -> "Completed"
            | Active    -> "Active"
        sprintf "%s Todos: \n\t%s\n\n" (filterName f)
    t |> filter |> format |> display

let rec createStore reducer init =
    let mutable state = init
    let mutable subs = Seq.empty
    let dispatcher (action : 'Payload) =
        state <- reducer state action
        subs |> Seq.iter (fun s -> s())
        action
    let subscriber subscriber =
        subs <- Seq.append subs [ subscriber ]
        let index = Seq.length subs
        fun () ->
            subs <- subs
                    |> Seq.mapi (fun i s -> i, s)
                    |> Seq.filter (fun (i, e) -> i <> index)
                    |> Seq.map (fun (i, s) -> s)
    let getState = fun () -> state
    {
      getState = getState
      dispatch = dispatcher
      subscribe = subscriber
    }

let initialState = { filter = All; todos = [] }
let store = createStore todoApp initialState
let printState () = printf "%s" (formatState <| store.getState())
let subscriber = store.subscribe(printState) |> ignore

store.dispatch(Add "todo 1") |> ignore
store.dispatch(Add "todo 2") |> ignore
store.dispatch(SetFilter Active) |> ignore
store.dispatch(Add "todo 3") |> ignore
store.dispatch(Toggle "todo 2") |> ignore
store.dispatch(SetFilter All) |> ignore