module WinAPI.Common

open System
open System.Runtime.InteropServices

type HANDLE = IntPtr
type DWORD = uint32
type WORD = uint16
type LPVOID = UIntPtr
type BOOL = bool
type LARGE_INTEGER = int64
//[<MarshalAs(UnmanagedType.PTR)>]
type ULONG_PTR = uint64
type SIZE_T = ULONG_PTR
type PVOID = UIntPtr
type ULONGLONG = uint64
type USHORT = uint16