module Job

#nowarn "9" //unverifiable IL Code in structs
open System
open System.Runtime.InteropServices
open System.Diagnostics

module Interop = 

    [<Struct>]
    [<StructLayout(LayoutKind.Sequential)>]
    type SECURITY_ATTRIBUTES = {
        nLength: uint32
        lpSecurityDescriptor: IntPtr 
        [<MarshalAs(UnmanagedType.Bool)>] bInheritHandle: bool
    }

    ///<see cref="https://msdn.microsoft.com/en-us/library/windows/desktop/ms682409(v=vs.85).aspx">Win API CreateJobObject</see>
    ///returns existing object of job it it existed
    [<DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)>] //Windows XP
    extern IntPtr CreateJobObject(IntPtr lpJobAttributes, string lpName)

    [<DllImport("Kernel32.dll", SetLastError = true)>] //Windows XP
    extern bool CloseHandle(IntPtr hObject)

    ///<see cref="https://msdn.microsoft.com/en-us/library/windows/desktop/ms681949(v=vs.85).aspx">AssignProcessToJobObject Win API</see>
    [<DllImport("Kernel32.dll", SetLastError = true)>] //Windows XP
    extern bool AssignProcessToJobObject(IntPtr hJob, IntPtr hProcess)

    ///<see cref="https://msdn.microsoft.com/en-us/library/windows/desktop/ms684312(v=vs.85).aspx">OpenJobObject Win API</see>
    [<DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)>] //Windows XP
    extern IntPtr OpenJobObject(uint32 dwDesiredAccess, bool bInheritHandles, string lpName)

    let JOB_OBJECT_ALL_ACCESS = 0x1F001Fu

    ///<see cref="https://msdn.microsoft.com/en-us/library/windows/desktop/ms684127(v=vs.85).aspx">OpenJobObject Win API</see>
    [<DllImport("Kernel32.dll", SetLastError = true)>] //Windows XP
    extern bool IsProcessInJob(IntPtr ProcessHandle, IntPtr JobHandle, bool& Result)

    ///<see cref="https://msdn.microsoft.com/en-us/library/windows/desktop/ms686709(v=vs.85).aspx">OpenJobObject Win API</see>
    [<DllImport("Kernel32.dll", SetLastError = true)>] //Windows XP
    extern bool TerminateJobObject(IntPtr hJob, uint32 uExitCode)

    type JOBOBJECTINFOCLASS = 
        | JobObjectAssociateCompletionPortInformation = 7
        //| JobObjectBasicLimitInformation = 2
        | JobObjectBasicUIRestrictions = 4
        | JobObjectCpuRateControlInformation = 15
        | JobObjectEndOfJobTimeInformation = 6
        | JobObjectExtendedLimitInformation = 9
        | JobObjectGroupInformation = 11
        | JobObjectGroupInformationEx = 14
        | JobObjectLimitViolationInformation2 = 35
        | JobObjectNetRateControlInformation = 32
        | JobObjectNotificationLimitInformation = 12
        | JobObjectNotificationLimitInformation2 = 34
        | JobObjectSecurityLimitInformation = 5

    [<Flags>]
    type JOBOBJECT_BASIC_LIMIT_INFORMATION_FLAGS = 
        | JOB_OBJECT_LIMIT_ACTIVE_PROCESS = 0x0008
        | JOB_OBJECT_LIMIT_AFFINITY = 0x0010
        | JOB_OBJECT_LIMIT_BREAKAWAY_OK = 0x0800
        | JOB_OBJECT_LIMIT_DIE_ON_UNHANDLED_EXCEPTION = 0x0400
        | JOB_OBJECT_LIMIT_JOB_MEMORY = 0x0200
        | JOB_OBJECT_LIMIT_JOB_TIME = 0x0004
        | JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE = 0x2000
        | JOB_OBJECT_LIMIT_PRESERVE_JOB_TIME = 0x0040
        | JOB_OBJECT_LIMIT_PRIORITY_CLASS = 0x0020
        | JOB_OBJECT_LIMIT_PROCESS_MEMORY = 0x0100
        | JOB_OBJECT_LIMIT_PROCESS_TIME = 0x0002
        | JOB_OBJECT_LIMIT_SCHEDULING_CLASS = 0x0080
        | JOB_OBJECT_LIMIT_SILENT_BREAKAWAY_OK = 0x1000
        | JOB_OBJECT_LIMIT_SUBSET_AFFINITY = 0x4000
        | JOB_OBJECT_LIMIT_WORKINGSET = 0x0001

    //[StructLayout(LayoutKind.Sequential)]
    //struct JOBOBJECT_BASIC_LIMIT_INFORMATION
    //{
    //    public Int64 PerProcessUserTimeLimit;
    //    public Int64 PerJobUserTimeLimit;
    //    public UInt32 LimitFlags;
    //    public UIntPtr MinimumWorkingSetSize;
    //    public UIntPtr MaximumWorkingSetSize;
    //    public UInt32 ActiveProcessLimit;
    //    public UIntPtr Affinity;
    //    public UInt32 PriorityClass;
    //    public UInt32 SchedulingClass;
    //}    
    
    [<Struct>]
    [<StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)>]
    type JOBOBJECT_BASIC_LIMIT_INFORMATION = { 
        PerProcessUserTimeLimit: int64
        PerJobUserTimeLimit: int64
        LimitFlags: JOBOBJECT_BASIC_LIMIT_INFORMATION_FLAGS
        MinimumWorkingSetSize: UIntPtr
        MaximumWorkingSetSize: UIntPtr
        ActiveProcessLimit: uint32
        Affinity: UIntPtr
        PriorityClass: uint32
        SchedulingClass: uint32        
    }
    with static member Default() = Unchecked.defaultof<JOBOBJECT_BASIC_LIMIT_INFORMATION>                    

    [<Struct>]
    [<StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)>]
    type JOBOBJECT_ASSOCIATE_COMPLETION_PORT = 
        {
            CompletionKey: IntPtr
            CompletionPort: IntPtr
        }

    [<Flags>]
    type JOBOBJECT_BASIC_UI_RESTRICTIONS_FLAGS = 
        | JOB_OBJECT_UILIMIT_DESKTOP = 0x00000040
        | JOB_OBJECT_UILIMIT_DISPLAYSETTINGS = 0x00000010
        | JOB_OBJECT_UILIMIT_EXITWINDOWS = 0x00000080
        | JOB_OBJECT_UILIMIT_GLOBALATOMS = 0x00000020
        | JOB_OBJECT_UILIMIT_HANDLES = 0x00000001
        | JOB_OBJECT_UILIMIT_READCLIPBOARD = 0x00000002
        | JOB_OBJECT_UILIMIT_SYSTEMPARAMETERS = 0x00000008
        | JOB_OBJECT_UILIMIT_WRITECLIPBOARD = 0x00000004

    [<Struct>]
    [<StructLayout(LayoutKind.Sequential)>]
    type JOBOBJECT_BASIC_UI_RESTRICTIONS = 
        {
            UIRestrictionsClass: JOBOBJECT_BASIC_UI_RESTRICTIONS_FLAGS
        }
        with static member Default() = Unchecked.defaultof<JOBOBJECT_BASIC_UI_RESTRICTIONS>

    [<Flags>]
    type JOBOBJECT_CPU_RATE_CONTROL_INFORMATION_FLAGS = 
        | JOB_OBJECT_CPU_RATE_CONTROL_ENABLE = 0x1
        | JOB_OBJECT_CPU_RATE_CONTROL_WEIGHT_BASED = 0x2
        | JOB_OBJECT_CPU_RATE_CONTROL_HARD_CAP = 0x4
        | JOB_OBJECT_CPU_RATE_CONTROL_NOTIFY = 0x8
        | JOB_OBJECT_CPU_RATE_CONTROL_MIN_MAX_RATE = 0x10


    [<Struct>]
    [<StructLayout(LayoutKind.Explicit, CharSet = CharSet.Auto)>]
    type JOBOBJECT_CPU_RATE_CONTROL_INFORMATION = {
        [<FieldOffset(0)>]
        ControlFlags: JOBOBJECT_CPU_RATE_CONTROL_INFORMATION_FLAGS
        [<FieldOffset(4)>]
        CpuRate: uint32
        [<FieldOffset(4)>]
        Weight: uint32
        [<FieldOffset(4)>]
        MinRate: uint16
        [<FieldOffset(6)>]
        MaxRate: uint16
    }
    with static member Default() = Unchecked.defaultof<JOBOBJECT_CPU_RATE_CONTROL_INFORMATION>

    [<Flags>]
    type JOBOBJECT_END_OF_JOB_TIME_INFORMATION_FLAGS = 
        | JOB_OBJECT_TERMINATE_AT_END_OF_JOB = 0
        | JOB_OBJECT_POST_AT_END_OF_JOB = 1

    [<Struct>]
    [<StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)>]
    type JOBOBJECT_END_OF_JOB_TIME_INFORMATION = {
        EndOfJobTimeAction: JOBOBJECT_END_OF_JOB_TIME_INFORMATION_FLAGS
    }

    [<Struct>]
    [<StructLayout(LayoutKind.Sequential)>]
    type IO_COUNTERS = {
        ReadOperationCount: uint64
        WriteOperationCount: uint64
        OtherOperationCount: uint64
        ReadTransferCount: uint64
        WriteTransferCount: uint64
        OtherTransferCount: uint64
    }
    with static member Default() = Unchecked.defaultof<IO_COUNTERS>  

    [<Struct>]
    [<StructLayout(LayoutKind.Sequential)>]
    type JOBOBJECT_EXTENDED_LIMIT_INFORMATION = {
        BasicLimitInformation: JOBOBJECT_BASIC_LIMIT_INFORMATION
        IoInfo: IO_COUNTERS
        ProcessMemoryLimit: UIntPtr
        JobMemoryLimit: UIntPtr
        PeakProcessMemoryUsed: UIntPtr
        PeakJobMemoryUsed: UIntPtr
    }
    with static member Default() = Unchecked.defaultof<JOBOBJECT_EXTENDED_LIMIT_INFORMATION>     

    ///<see cref="https://msdn.microsoft.com/en-us/library/windows/desktop/ms686216(v=vs.85).aspx">OpenJobObject Win API</see>
    [<DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)>] //Windows XP
    extern bool SetInformationJobObject(IntPtr hJob, JOBOBJECTINFOCLASS JobObjectInfoClass, IntPtr lpJobObjectInfo, uint32 cbJobObjectInfoLength)

    ///<see cref="https://msdn.microsoft.com/en-us/library/windows/desktop/ms683213(v=vs.85).aspx">Affinity mask</see>
    [<DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)>]
    extern bool GetProcessAffinityMask(IntPtr hProcess, [<In;Out>] UIntPtr lpProcessAffinityMask, [<In;Out>] UIntPtr lpSystemAffinityMask)

    [<Struct>]
    [<StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)>]
    type MEMORYSTATUSEX = 
        [<DefaultValue()>]
        val mutable dwLength: uint32
        [<DefaultValue()>]
        val mutable dwMemoryLoad: uint32
        [<DefaultValue()>]
        val mutable ullTotalPhys: uint64
        [<DefaultValue()>]
        val mutable ullAvailPhys: uint64
        [<DefaultValue()>]
        val mutable ullTotalPageFile: uint64
        [<DefaultValue()>]
        val mutable ullAvailPageFile: uint64
        [<DefaultValue()>]
        val mutable ullTotalVirtual: uint64
        [<DefaultValue()>]
        val mutable ullAvailVirtual: uint64
        [<DefaultValue()>]
        val mutable ullAvailExtendedVirtual: uint64
        static member create() = 
            new MEMORYSTATUSEX(
                dwLength = uint32(Marshal.SizeOf(typeof<MEMORYSTATUSEX>)),
                dwMemoryLoad = 0u,
                ullTotalPhys = 0uL,
                ullAvailPhys = 0uL,
                ullTotalPageFile = 0uL,
                ullAvailPageFile = 0uL,
                ullTotalVirtual = 0uL,
                ullAvailVirtual = 0uL,
                ullAvailExtendedVirtual = 0uL)    


    [<DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)>]
    extern [<MarshalAs(UnmanagedType.Bool)>] bool GlobalMemoryStatusEx([<In; Out>] IntPtr lpBuffer);

open Interop
open System.ComponentModel

[<Struct>]
type JobState = {
        cpuLimit: JOBOBJECT_CPU_RATE_CONTROL_INFORMATION
        limit: JOBOBJECT_EXTENDED_LIMIT_INFORMATION
        uiLimit: JOBOBJECT_BASIC_UI_RESTRICTIONS
        limitsApplied: bool
    }
    with static member Default() = Unchecked.defaultof<JobState>

module Measures = 
    [<Measure>] type KB
    [<Measure>] type MB
    [<Measure>] type GB
    [<Measure>] type ``%``

open Measures
open System.Security.AccessControl

[<Interface>]
type IJob = 
    inherit IDisposable
    abstract StartProcess: string * ?args: string -> IJob
    abstract Terminate: ?code: uint32 -> unit
    //abstract GetProcesses: Process list
    abstract LimitCPU: percent: int<``%``> -> IJob
    abstract LimitRamPercent: percent: int<``%``> -> IJob
    abstract LimitRamMB: mbytes: int<MB> -> IJob
    abstract LimitWorkingSet: percent: byte -> IJob
    abstract LimitCores: count: byte -> IJob
    abstract DisableUI: unit -> IJob
    abstract KillOnClose: unit -> IJob

exception CouldNotAddProcess
exception WrongLimit of string

let rec private build (job: IntPtr) (state: JobState) = 
    {        
        new IJob with
            member x.Terminate(code) = 
                TerminateJobObject(job, defaultArg code (uint32(-1))) |> ignore ///TODO - think
            member x.Dispose() =  
                //x.Terminate() //TODO: think
                CloseHandle(job) |> ignore
            //member x.GetProcesses = state.attachedProcesses
            member x.StartProcess(name, ?args) = 
                let x = 
                    if not state.limitsApplied then 
                        let mutable cpuLimits = state.cpuLimit
                        let cpuLimitsAddress = NativeInterop.NativePtr.toNativeInt(&&cpuLimits)
                        if not(SetInformationJobObject(job, 
                                JOBOBJECTINFOCLASS.JobObjectCpuRateControlInformation,  
                                    cpuLimitsAddress, uint32(sizeof<JOBOBJECT_CPU_RATE_CONTROL_INFORMATION>))) then 
                            raise(Win32Exception(Marshal.GetLastWin32Error()))
                        let mutable memoryLimit = state.limit
                        let memoryLimitAddress = NativeInterop.NativePtr.toNativeInt(&&memoryLimit)
                        if not(SetInformationJobObject(job, 
                                JOBOBJECTINFOCLASS.JobObjectExtendedLimitInformation,  
                                    memoryLimitAddress, uint32(Marshal.SizeOf(typeof<JOBOBJECT_EXTENDED_LIMIT_INFORMATION>)))) then 
                            raise(Win32Exception(Marshal.GetLastWin32Error()))    
                        let mutable settings = state.uiLimit
                        if state.limitsApplied then 
                            let settingsAddress = NativeInterop.NativePtr.toNativeInt(&&settings)
                            if not(SetInformationJobObject(job, 
                                    JOBOBJECTINFOCLASS.JobObjectBasicUIRestrictions,  
                                        settingsAddress, uint32(sizeof<JOBOBJECT_BASIC_UI_RESTRICTIONS>))) then 
                                raise(Win32Exception(Marshal.GetLastWin32Error()))  
                        build job {state with limitsApplied = true; cpuLimit = cpuLimits; limit = memoryLimit; uiLimit = settings}
                    else x
                let prInfo = 
                    match args with
                    | None -> ProcessStartInfo(name)
                    | Some args -> ProcessStartInfo(name, args)
                prInfo.UseShellExecute <- true //!important //TODO: set default controlable settings
                use pr = new Process(StartInfo = prInfo) //warn - here we used NOT use but let
                if not(pr.Start()) then
                    raise CouldNotAddProcess
                if not(AssignProcessToJobObject(job, pr.Handle)) then 
                    raise(Win32Exception(Marshal.GetLastWin32Error()))
                x
            member x.LimitCPU(percent) = 
                if (percent < 5<``%``> || percent >= 100<``%``>) then 
                    raise <| WrongLimit (sprintf "CPU: %d" percent)
                let rate = (uint32 percent) * 100u
                let mutable cpuLimits = 
                    { state.cpuLimit with 
                        ControlFlags =                             
                            (state.cpuLimit.ControlFlags ||| 
                                JOBOBJECT_CPU_RATE_CONTROL_INFORMATION_FLAGS.JOB_OBJECT_CPU_RATE_CONTROL_ENABLE |||
                                JOBOBJECT_CPU_RATE_CONTROL_INFORMATION_FLAGS.JOB_OBJECT_CPU_RATE_CONTROL_HARD_CAP)
                                &&& (~~~JOBOBJECT_CPU_RATE_CONTROL_INFORMATION_FLAGS.JOB_OBJECT_CPU_RATE_CONTROL_WEIGHT_BASED)
                                &&& (~~~JOBOBJECT_CPU_RATE_CONTROL_INFORMATION_FLAGS.JOB_OBJECT_CPU_RATE_CONTROL_MIN_MAX_RATE)
                        CpuRate = rate
                        MinRate = uint16(rate &&& 0xffffu)
                        MaxRate = uint16(rate >>> 16)
                        Weight = rate
                    }
                if state.limitsApplied then 
                    let cpuLimitsAddress = NativeInterop.NativePtr.toNativeInt(&&cpuLimits)
                    if not(SetInformationJobObject(job, 
                            JOBOBJECTINFOCLASS.JobObjectCpuRateControlInformation,  
                                cpuLimitsAddress, uint32(sizeof<JOBOBJECT_CPU_RATE_CONTROL_INFORMATION>))) then 
                        raise(Win32Exception(Marshal.GetLastWin32Error()))
                build job {state with cpuLimit = cpuLimits}
            member x.KillOnClose() = 
                let mutable memoryLimit = 
                    { state.limit with
                        BasicLimitInformation =
                            { state.limit.BasicLimitInformation with
                                LimitFlags = state.limit.BasicLimitInformation.LimitFlags ||| JOBOBJECT_BASIC_LIMIT_INFORMATION_FLAGS.JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE 
                            }
                    }
                if state.limitsApplied then 
                    let memoryLimitAddress = NativeInterop.NativePtr.toNativeInt(&&memoryLimit)
                    if not(SetInformationJobObject(job, 
                            JOBOBJECTINFOCLASS.JobObjectExtendedLimitInformation,  
                                memoryLimitAddress, uint32(Marshal.SizeOf(typeof<JOBOBJECT_EXTENDED_LIMIT_INFORMATION>)))) then 
                        raise(Win32Exception(Marshal.GetLastWin32Error()))     
                build job {state with limit = memoryLimit}
            member x.LimitWorkingSet(percent) = 
                if (percent < 5uy || percent >= 100uy) then 
                    raise <| WrongLimit (sprintf "RAM: %d" percent)
                let mutable mem = MEMORYSTATUSEX.create()
                if (not(GlobalMemoryStatusEx(NativeInterop.NativePtr.toNativeInt(&&mem)))) then
                    raise(Win32Exception(Marshal.GetLastWin32Error()))
                let allocatedForJob = UIntPtr(uint64(double mem.ullTotalPhys * (double percent) / 100.0))
                let mutable memoryLimit = 
                    { state.limit with
                        BasicLimitInformation =
                            { state.limit.BasicLimitInformation with
                                LimitFlags = state.limit.BasicLimitInformation.LimitFlags ||| JOBOBJECT_BASIC_LIMIT_INFORMATION_FLAGS.JOB_OBJECT_LIMIT_WORKINGSET 
                                MaximumWorkingSetSize = allocatedForJob
                            }
                    }
                if state.limitsApplied then 
                    let memoryLimitAddress = NativeInterop.NativePtr.toNativeInt(&&memoryLimit)
                    if not(SetInformationJobObject(job, 
                            JOBOBJECTINFOCLASS.JobObjectExtendedLimitInformation,  
                                memoryLimitAddress, uint32(Marshal.SizeOf(typeof<JOBOBJECT_EXTENDED_LIMIT_INFORMATION>)))) then 
                        raise(Win32Exception(Marshal.GetLastWin32Error()))     
                build job {state with limit = memoryLimit}
            member x.LimitRamPercent(percent) = 
                if (percent < 5<``%``> || percent >= 100<``%``>) then 
                    raise <| WrongLimit (sprintf "RAM: %d" percent)
                let mutable mem = MEMORYSTATUSEX.create()
                if (not(GlobalMemoryStatusEx(NativeInterop.NativePtr.toNativeInt(&&mem)))) then
                    raise(Win32Exception(Marshal.GetLastWin32Error()))
                let allocatedForJob = uint64(double mem.ullTotalPhys * (double percent) / 100.0)
                let mutable memoryLimit = 
                    { state.limit with
                        BasicLimitInformation =
                            { state.limit.BasicLimitInformation with
                                LimitFlags = state.limit.BasicLimitInformation.LimitFlags ||| JOBOBJECT_BASIC_LIMIT_INFORMATION_FLAGS.JOB_OBJECT_LIMIT_JOB_MEMORY 
                            }
                        JobMemoryLimit = UIntPtr(allocatedForJob)
                        PeakJobMemoryUsed = UIntPtr(allocatedForJob)
                    }
                if state.limitsApplied then 
                    let memoryLimitAddress = NativeInterop.NativePtr.toNativeInt(&&memoryLimit)
                    if not(SetInformationJobObject(job, 
                            JOBOBJECTINFOCLASS.JobObjectExtendedLimitInformation,  
                                memoryLimitAddress, uint32(sizeof<JOBOBJECT_EXTENDED_LIMIT_INFORMATION>))) then 
                        raise(Win32Exception(Marshal.GetLastWin32Error()))     
                build job {state with limit = memoryLimit}
            member x.LimitRamMB(mbytes:int<MB>) = 
                if mbytes < 100<MB> then raise <| WrongLimit(sprintf "RAM: %d, min 100MB" mbytes)
                let bytes = uint64(mbytes * 1<1/MB>) * 1024uL * 1024uL
                let mutable mem = MEMORYSTATUSEX.create()
                if (not(GlobalMemoryStatusEx(NativeInterop.NativePtr.toNativeInt(&&mem)))) then
                    raise(Win32Exception(Marshal.GetLastWin32Error()))
                if bytes >= mem.ullTotalPhys then 
                    x
                else 
                    let mutable memoryLimit = 
                        { state.limit with
                            BasicLimitInformation =
                                { state.limit.BasicLimitInformation with
                                    LimitFlags = state.limit.BasicLimitInformation.LimitFlags ||| JOBOBJECT_BASIC_LIMIT_INFORMATION_FLAGS.JOB_OBJECT_LIMIT_JOB_MEMORY 
                                }
                            JobMemoryLimit = UIntPtr(bytes)
                            PeakJobMemoryUsed = UIntPtr(bytes)
                        }
                    if state.limitsApplied then 
                        let memoryLimitAddress = NativeInterop.NativePtr.toNativeInt(&&memoryLimit)
                        if not(SetInformationJobObject(job, 
                                JOBOBJECTINFOCLASS.JobObjectExtendedLimitInformation,  
                                    memoryLimitAddress, uint32(sizeof<JOBOBJECT_EXTENDED_LIMIT_INFORMATION>))) then 
                            raise(Win32Exception(Marshal.GetLastWin32Error()))     
                    build job {state with limit = memoryLimit}
            member x.DisableUI() = 
                let mutable settings = 
                    { state.uiLimit with
                        UIRestrictionsClass = 
                            state.uiLimit.UIRestrictionsClass |||
                            JOBOBJECT_BASIC_UI_RESTRICTIONS_FLAGS.JOB_OBJECT_UILIMIT_DESKTOP |||
                            JOBOBJECT_BASIC_UI_RESTRICTIONS_FLAGS.JOB_OBJECT_UILIMIT_DISPLAYSETTINGS |||
                            JOBOBJECT_BASIC_UI_RESTRICTIONS_FLAGS.JOB_OBJECT_UILIMIT_EXITWINDOWS |||
                            JOBOBJECT_BASIC_UI_RESTRICTIONS_FLAGS.JOB_OBJECT_UILIMIT_GLOBALATOMS |||
                            JOBOBJECT_BASIC_UI_RESTRICTIONS_FLAGS.JOB_OBJECT_UILIMIT_HANDLES |||
                            JOBOBJECT_BASIC_UI_RESTRICTIONS_FLAGS.JOB_OBJECT_UILIMIT_READCLIPBOARD |||
                            JOBOBJECT_BASIC_UI_RESTRICTIONS_FLAGS.JOB_OBJECT_UILIMIT_SYSTEMPARAMETERS |||
                            JOBOBJECT_BASIC_UI_RESTRICTIONS_FLAGS.JOB_OBJECT_UILIMIT_WRITECLIPBOARD
                    }
                if state.limitsApplied then 
                    let settingsAddress = NativeInterop.NativePtr.toNativeInt(&&settings)
                    if not(SetInformationJobObject(job, 
                            JOBOBJECTINFOCLASS.JobObjectBasicUIRestrictions,  
                                settingsAddress, uint32(sizeof<JOBOBJECT_BASIC_UI_RESTRICTIONS>))) then 
                        raise(Win32Exception(Marshal.GetLastWin32Error()))   
                build job {state with uiLimit = settings}
            member x.LimitCores(count) = 
                if count = 0uy then 
                    raise <| WrongLimit(sprintf "Cores: %d" count)
                let mutable processAffinity = 0uL
                let mutable systemAffinity = 0uL
                if not(GetProcessAffinityMask(Process.GetCurrentProcess().Handle, 
                                                unativeint(NativeInterop.NativePtr.toNativeInt(&&processAffinity)), 
                                                unativeint(NativeInterop.NativePtr.toNativeInt(&&systemAffinity)))) then 
                    raise(Win32Exception(Marshal.GetLastWin32Error()))
                let rand = Random()
                let setOfProcessIndexes = 
                    [
                        for i in 0..(sizeof<uint64> - 1) do 
                            if ((processAffinity >>> i) &&& 0x1uL) = 0x1uL then 
                                yield i
                    ] 
                if int32 count >= setOfProcessIndexes.Length then 
                    x
                else 
                    let affinityMask = 
                        setOfProcessIndexes |> List.sortBy(fun _ -> rand.NextDouble()) |> List.take(int32 count)
                        |> List.fold(fun acc position -> (0x1uL <<< position) ||| acc) 0x0uL
                    let mutable affinityLimit = 
                        { state.limit with
                            BasicLimitInformation =
                                { state.limit.BasicLimitInformation with
                                    LimitFlags = state.limit.BasicLimitInformation.LimitFlags ||| JOBOBJECT_BASIC_LIMIT_INFORMATION_FLAGS.JOB_OBJECT_LIMIT_AFFINITY 
                                    Affinity = UIntPtr(affinityMask)
                                }
                        }
                    if state.limitsApplied then 
                        let affinityLimitAddress = NativeInterop.NativePtr.toNativeInt(&&affinityLimit)
                        if not(SetInformationJobObject(job, 
                                JOBOBJECTINFOCLASS.JobObjectExtendedLimitInformation,  
                                    affinityLimitAddress, uint32(sizeof<JOBOBJECT_EXTENDED_LIMIT_INFORMATION>))) then 
                            raise(Win32Exception(Marshal.GetLastWin32Error()))                    
                    build job {state with limit = affinityLimit}
    }

//let find name = 
//    let job = OpenJobObject(JOB_OBJECT_ALL_ACCESS, true, name)
//    if IntPtr.Zero = job then 
//        let err = Marshal.GetLastWin32Error()
//        if err = 0 || err = 2 then         
//            None
//        else failwithf "Win32 err: %d" err
//    else Some(build job)

let create name = 
    let job = CreateJobObject(IntPtr.Zero, name) //TODO: security
    build job <| JobState.Default()