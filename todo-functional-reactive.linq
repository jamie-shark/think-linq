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

let rec applyAt predicate selector = function
    | []    -> []
    | x::xs -> if  predicate x
               then selector x :: applyAt predicate selector xs
               else          x :: applyAt predicate selector xs

let removeItemAt index xs =
    xs
    |> Seq.mapi (fun i x -> i, x)
    |> Seq.filter (fun (i, _) -> i <> index)
    |> Seq.map (fun (i, x) -> x)

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

let todoApp state message =
    let filterReducer filter = function
        | SetFilter f -> f
        | _           -> filter
    let todosReducer todos = function
        | Add m        -> todos @ [{ message = m; completed = false }]
        | Toggle index -> applyAt (fun todo -> todo.message = index) (fun todo -> { todo with completed = not todo.completed }) todos
        | _            -> todos
    {
      filter = filterReducer state.filter message
      todos  = todosReducer state.todos message
    }

let rec createStore reducer init =
    let mutable state = init
    let mutable subscribers = Seq.empty
    let dispatcher action =
        state <- reducer state action
        subscribers |> Seq.iter (fun sub -> sub())
        action
    let unsub index () = subscribers <- removeItemAt index subscribers
    let subscriber subscriber =
        subscribers <- Seq.append subscribers [ subscriber ]
        Seq.length subscribers |> (-) 1 |> unsub
    let getState = fun () -> state
    {
      getState = getState
      dispatch = dispatcher
      subscribe = subscriber
    }

let initialState = { filter = All; todos = [] }
let store = createStore todoApp initialState

let consoleSubscription =
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
    (fun () -> () |> store.getState |> formatState |> printf "%s")
    |> store.subscribe

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

consoleSubscription ()