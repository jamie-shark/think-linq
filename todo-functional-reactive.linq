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

let filterTodos = function
    | All       -> id
    | Completed -> Seq.filter (fun todo -> todo.completed)
    | Active    -> Seq.filter (fun todo -> not todo.completed)

type State = { filter: Filter; todos: Todo list }

type Action =
    | Add of string
    | Toggle of string
    | SetFilter of Filter

type Store<'State, 'Payload> =
    {
      getState : unit -> 'State
      dispatch : 'Payload -> 'Payload
      subscribe : (unit -> unit) -> unit -> unit
    }

let rec applyAt predicate selector = function
    | []    -> []
    | x::xs -> if  predicate x
               then selector x :: applyAt predicate selector xs
               else          x :: applyAt predicate selector xs

let filterReducer filter = function
    | SetFilter f -> f
    | _           -> filter

let todosReducer todos = function
    | Add m        -> todos @ [{ message = m; completed = false }]
    | Toggle index -> applyAt (fun todo -> todo.message = index) (fun todo -> { todo with completed = not todo.completed }) todos
    | _            -> todos

let todoApp state message =
    {
      filter = filterReducer state.filter message
      todos  = todosReducer state.todos message
    }

let formatState state =
    let formatTodo todo =
        let statusMessage = function
            | false -> " - Incomplete!"
            | _ -> ""
        statusMessage todo.completed
        |> sprintf "* '%s'%s" todo.message
    let itemDelimiter = "\n\t"
    state.todos
    |> filterTodos state.filter
    |> Seq.map formatTodo
    |> String.concat itemDelimiter
    |> sprintf "%A Todos: %s%s\n\n" state.filter itemDelimiter

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

let subscriber =
    (fun () -> () |> store.getState |> formatState |> printf "%s")
    |> store.subscribe
    |> ignore

let dispatch action =
    action
    |> store.dispatch
    |> Dump
    |> ignore

Add "todo 1"     |> dispatch
Add "todo 2"     |> dispatch
SetFilter Active |> dispatch
Add "todo 3"     |> dispatch
Toggle "todo 2"  |> dispatch
SetFilter All    |> dispatch