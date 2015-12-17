using System;
using System.Runtime.InteropServices;

namespace FTDIChip.FTD2XX
{
	public enum FT_STATUS
	{
		FT_OK = 0,
		FT_INVALID_HANDLE,
		FT_DEVICE_NOT_FOUND,
		FT_DEVICE_NOT_OPENED,
		FT_IO_ERROR,
		FT_INSUFFICIENT_RESOURCES,
		FT_INVALID_PARAMETER,
		FT_INVALID_BAUD_RATE,
		FT_DEVICE_NOT_OPENED_FOR_ERASE,
		FT_DEVICE_NOT_OPENED_FOR_WRITE,
		FT_FAILED_TO_WRITE_DEVICE,
		FT_EEPROM_READ_FAILED,
		FT_EEPROM_WRITE_FAILED,
		FT_EEPROM_ERASE_FAILED,
		FT_EEPROM_NOT_PRESENT,
		FT_EEPROM_NOT_PROGRAMMED,
		FT_INVALID_ARGS,
		FT_OTHER_ERROR}

	;

	static public class NativeMethods
	{
		[DllImport ("FTD2XX.dll")]
		internal static extern FT_STATUS FT_Open (UInt32 uiPort, ref uint ftHandle);

		[DllImport ("FTD2XX.dll")]
		internal static extern FT_STATUS FT_Close (uint ftHandle);

		[DllImport ("FTD2XX.dll")]
		internal static extern FT_STATUS FT_Read (uint ftHandle, IntPtr lpBuffer, UInt32 dwBytesToRead, ref UInt32 lpdwBytesReturned);

		[DllImport ("FTD2XX.dll")]
		internal static extern FT_STATUS FT_Write (uint ftHandle, IntPtr lpBuffer, UInt32 dwBytesToRead, ref UInt32 lpdwBytesWritten);

		[DllImport ("FTD2XX.dll")]
		internal static extern FT_STATUS FT_SetDataCharacteristics (uint ftHandle, byte uWordLength, byte uStopBits, byte uParity);

		[DllImport ("FTD2XX.dll")]
		internal static extern FT_STATUS FT_SetFlowControl (uint ftHandle, char usFlowControl, byte uXon, byte uXoff);

		[DllImport ("FTD2XX.dll")]
		internal static extern FT_STATUS FT_GetModemStatus (uint ftHandle, ref UInt32 lpdwModemStatus);

		[DllImport ("FTD2XX.dll")]
		internal static extern FT_STATUS FT_Purge (uint ftHandle, UInt32 dwMask);

		[DllImport ("FTD2XX.dll")]
		internal static extern FT_STATUS FT_ClrRts (uint ftHandle);

		[DllImport ("FTD2XX.dll")]
		internal static extern FT_STATUS FT_SetBreakOn (uint ftHandle);

		[DllImport ("FTD2XX.dll")]
		internal static extern FT_STATUS FT_SetBreakOff (uint ftHandle);

		[DllImport ("FTD2XX.dll")]
		internal static extern FT_STATUS FT_GetStatus (uint ftHandle, ref UInt32 lpdwAmountInRxQueue, ref UInt32 lpdwAmountInTxQueue, ref UInt32 lpdwEventStatus);

		[DllImport ("FTD2XX.dll")]
		internal static extern FT_STATUS FT_ResetDevice (uint ftHandle);

		[DllImport ("FTD2XX.dll")]
		internal static extern FT_STATUS FT_SetDivisor (uint ftHandle, char usDivisor);
	}
}

