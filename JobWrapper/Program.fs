// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open System
open System
open Job.Measures

[<EntryPoint>]
let main argv = 
    let name = "Test_new_API_dvitel"
    //match Job.find name with
    //| Some job -> 
    //    job.Terminate()
    //    job.Dispose()
    //    printfn "Deleting job"
    //| _ -> 
    //    printfn "No job with this name"
    do 
        use job = 
            Job.create(name)
                //.LimitCores(1uy)
                .KillOnClose()
                .LimitRamMB(100<MB>)
                .LimitCPU(10<``%``>)
                .StartProcess("cmd.exe", "/K fsi.exe ..\\..\\inf.fsx")
                //.DisableUI()  
    
        //let job = CreateJobObject(IntPtr.Zero, null)
        //let mutable r = JOBOBJECT_EXTENDED_LIMIT_INFORMATION.Default()
        //r.BasicLimitInformation.LimitFlags <- uint32 JOBOBJECT_BASIC_LIMIT_INFORMATION_FLAGS.JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE
        //if SetInformationJobObject(job, JOBOBJECTINFOCLASS.JobObjectExtendedLimitInformation, NativeInterop.NativePtr.toNativeInt(&&r), uint32(sizeof<JOBOBJECT_EXTENDED_LIMIT_INFORMATION>)) then 
        //    printfn "OK"
        //else printfn "NOT OK"
    
        printfn "Done"
        Console.ReadKey() |> ignore
    printfn "Done 2"
    Console.ReadKey() |> ignore
    0 // return an integer exit code
