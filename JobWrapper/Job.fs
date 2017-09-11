module Job

#nowarn "9" //unverifiable IL Code in structs
open System
open System.Runtime.InteropServices
open WinAPI.Common
open System.Diagnostics

module Interop = 

    [<Struct>]
    [<StructLayout(LayoutKind.Sequential)>]
    type SECURITY_ATTRIBUTES = {
        nLength: DWORD
        lpSecurityDescriptor: LPVOID 
        bInheritHandle: BOOL
    }

    ///<see cref="https://msdn.microsoft.com/en-us/library/windows/desktop/ms682409(v=vs.85).aspx">Win API CreateJobObject</see>
    ///returns existing object of job it it existed
    [<DllImport("Kernel32.dll")>] //Windows XP
    extern HANDLE CreateJobObject([<MarshalAs(UnmanagedType.LPStruct)>]IntPtr lpJobAttributes, [<MarshalAs(UnmanagedType.LPTStr)>]string lpName)

    [<DllImport("Kernel32.dll")>] //Windows XP
    extern BOOL CloseHandle(HANDLE hObject)

    ///<see cref="https://msdn.microsoft.com/en-us/library/windows/desktop/ms681949(v=vs.85).aspx">AssignProcessToJobObject Win API</see>
    [<DllImport("Kernel32.dll")>] //Windows XP
    extern BOOL AssignProcessToJobObject(HANDLE hJob, HANDLE hProcess)

    ///<see cref="https://msdn.microsoft.com/en-us/library/windows/desktop/ms684312(v=vs.85).aspx">OpenJobObject Win API</see>
    [<DllImport("Kernel32.dll")>] //Windows XP
    extern HANDLE OpenJobObject(DWORD dwDesiredAccess, BOOL bInheritHandles, [<MarshalAs(UnmanagedType.LPTStr)>]string lpName)

    let JOB_OBJECT_ALL_ACCESS = 0x1F001Fu

    ///<see cref="https://msdn.microsoft.com/en-us/library/windows/desktop/ms684127(v=vs.85).aspx">OpenJobObject Win API</see>
    [<DllImport("Kernel32.dll")>] //Windows XP
    extern BOOL IsProcessInJob(HANDLE ProcessHandle, HANDLE JobHandle, BOOL& Result)

    ///<see cref="https://msdn.microsoft.com/en-us/library/windows/desktop/ms686709(v=vs.85).aspx">OpenJobObject Win API</see>
    [<DllImport("Kernel32.dll")>] //Windows XP
    extern BOOL TerminateJobObject(HANDLE hJob, [<MarshalAs(UnmanagedType.SysUInt)>]int uExitCode)

    type JOBOBJECTINFOCLASS = 
        | JobObjectAssociateCompletionPortInformation = 7
        | JobObjectBasicLimitInformation = 2
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
        | JOB_OBJECT_LIMIT_ACTIVE_PROCESS = 0x00000008
        | JOB_OBJECT_LIMIT_AFFINITY = 0x00000010
        | JOB_OBJECT_LIMIT_BREAKAWAY_OK = 0x00000800
        | JOB_OBJECT_LIMIT_DIE_ON_UNHANDLED_EXCEPTION = 0x00000400
        | JOB_OBJECT_LIMIT_JOB_MEMORY = 0x00000200
        | JOB_OBJECT_LIMIT_JOB_TIME = 0x00000004
        | JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE = 0x00002000
        | JOB_OBJECT_LIMIT_PRESERVE_JOB_TIME = 0x00000040
        | JOB_OBJECT_LIMIT_PRIORITY_CLASS = 0x00000020
        | JOB_OBJECT_LIMIT_PROCESS_MEMORY = 0x00000100
        | JOB_OBJECT_LIMIT_PROCESS_TIME = 0x00000002
        | JOB_OBJECT_LIMIT_SCHEDULING_CLASS = 0x00000080
        | JOB_OBJECT_LIMIT_SILENT_BREAKAWAY_OK = 0x00001000
        | JOB_OBJECT_LIMIT_SUBSET_AFFINITY = 0x00004000
        | JOB_OBJECT_LIMIT_WORKINGSET = 0x00000001

    [<Struct>]
    [<StructLayout(LayoutKind.Sequential)>]
    type JOBOBJECT_BASIC_LIMIT_INFORMATION = 
        {
          PerProcessUserTimeLimit: LARGE_INTEGER
          PerJobUserTimeLimit: LARGE_INTEGER
          LimitFlags: JOBOBJECT_BASIC_LIMIT_INFORMATION_FLAGS
          MinimumWorkingSetSize: SIZE_T
          MaximumWorkingSetSize: SIZE_T
          ActiveProcessLimit: DWORD
          Affinity: ULONG_PTR
          PriorityClass: DWORD
          SchedulingClass: DWORD
        }

    [<Struct>]
    [<StructLayout(LayoutKind.Sequential)>]
    type JOBOBJECT_ASSOCIATE_COMPLETION_PORT = 
        {
            CompletionKey: PVOID
            CompletionPort: HANDLE
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

    [<Flags>]
    type JOBOBJECT_CPU_RATE_CONTROL_INFORMATION_FLAGS = 
        | JOB_OBJECT_CPU_RATE_CONTROL_ENABLE = 0x1
        | JOB_OBJECT_CPU_RATE_CONTROL_WEIGHT_BASED = 0x2
        | JOB_OBJECT_CPU_RATE_CONTROL_HARD_CAP = 0x4
        | JOB_OBJECT_CPU_RATE_CONTROL_NOTIFY = 0x8
        | JOB_OBJECT_CPU_RATE_CONTROL_MIN_MAX_RATE = 0x10

    [<Struct>]
    [<StructLayout(LayoutKind.Explicit)>]
    type JOBOBJECT_CPU_RATE_CONTROL_INFORMATION = 
        {
            [<FieldOffset(0)>]
            ControlFlags: JOBOBJECT_CPU_RATE_CONTROL_INFORMATION_FLAGS
            [<FieldOffset(4)>]
            CpuRate: DWORD
            [<FieldOffset(4)>]
            Weight: DWORD
            [<FieldOffset(4)>]
            MinRate: WORD
            [<FieldOffset(6)>]
            MaxRate: WORD
        }

    [<Flags>]
    type JOBOBJECT_END_OF_JOB_TIME_INFORMATION_FLAGS = 
        | JOB_OBJECT_TERMINATE_AT_END_OF_JOB = 0
        | JOB_OBJECT_POST_AT_END_OF_JOB = 1

    [<Struct>]
    [<StructLayout(LayoutKind.Sequential)>]
    type JOBOBJECT_END_OF_JOB_TIME_INFORMATION = {
        EndOfJobTimeAction: JOBOBJECT_END_OF_JOB_TIME_INFORMATION_FLAGS
    }

    [<Struct>]
    [<StructLayout(LayoutKind.Sequential)>]
    type IO_COUNTERS = {
        ReadOperationCount: ULONGLONG
        WriteOperationCount: ULONGLONG
        OtherOperationCount: ULONGLONG
        ReadTransferCount: ULONGLONG
        WriteTransferCount: ULONGLONG
        OtherTransferCount: ULONGLONG
    }

    [<Struct>]
    [<StructLayout(LayoutKind.Sequential)>]
    type JOBOBJECT_EXTENDED_LIMIT_INFORMATION = {
        BasicLimitInformation: JOBOBJECT_BASIC_LIMIT_INFORMATION
        IoInfo: IO_COUNTERS
        ProcessMemoryLimit: SIZE_T
        JobMemoryLimit: SIZE_T
        PeakProcessMemoryUsed: SIZE_T
        PeakJobMemoryUsed: SIZE_T
    }

    ///<see cref="https://msdn.microsoft.com/en-us/library/windows/desktop/ms686216(v=vs.85).aspx">OpenJobObject Win API</see>
    [<DllImport("Kernel32.dll")>] //Windows XP
    extern BOOL SetInformationJobObject(HANDLE hJob, JOBOBJECTINFOCLASS JobObjectInfoClass, LPVOID lpJobObjectInfo, DWORD cbJobObjectInfoLength)

open Interop

[<Interface>]
type IJob = 
    inherit IDisposable
    abstract AssignProcess: Process -> bool
    abstract Start: ProcessStartInfo -> Process
    abstract Has: Process -> bool
    abstract Terminate: int -> unit
    //abstract SetLimit

let private build (job: HANDLE) = 
    {        
        new IJob with 
            member x.Dispose() =
                CloseHandle(job) |> ignore
            member x.AssignProcess pr =
                AssignProcessToJobObject(job, pr.Handle)
            member x.Start prInfo = 
                let pr = Process.Start prInfo //warn - here we used NOT use but let
                if x.AssignProcess pr then                    
                    pr
                else 
                    pr.Kill() |> ignore
                    null
            member x.Has pr = 
                let mutable res = false
                IsProcessInJob(pr.Handle, job, &res) |> ignore
                res
            member x.Terminate code = 
                TerminateJobObject(job, code) |> ignore
    }

let find name = 
    let job = OpenJobObject(JOB_OBJECT_ALL_ACCESS, true, name)
    build job

let create name = 
    let job = CreateJobObject(IntPtr.Zero, name) //TODO: security
    build job
