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

type Car(make : string, model : string, year : int) =
    member this.Make = make
    member this.Model = model
    member this.Year = year
    member this.WheelCount = 4
    
type Cat() =
    let mutable age = 3
    let mutable name = System.String.Empty
    
    member this.Purr() = printfn "Purrr"
    member this.Age
        with get() = age
        and set(v) = age <- v
    member this.Name
        with get() = name
        and set(v) = name <- v
        
let printProperties x =
    let t = x.GetType()
    let properties = t.GetProperties()
    printfn "-----------"
    printfn "%s" t.FullName
    properties |> Array.iter (fun prop ->
        if prop.CanRead then
            let value = prop.GetValue(x, null)
            printfn "%s: %O" prop.Name value
        else
            printfn "%s: ?" prop.Name)

let carInstance = new Car("Ford", "Focus", 2009)
let catInstance =

    let temp = new Cat()
    temp.Name <- "Mittens"
    temp
    
printProperties carInstance
printProperties catInstance
