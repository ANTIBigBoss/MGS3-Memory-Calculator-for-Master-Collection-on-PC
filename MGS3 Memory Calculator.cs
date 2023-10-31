using System;
using System.Diagnostics;

class Program
{
    // Constant for the name of the process we're looking for.
    const string PROCESS_NAME = "METAL GEAR SOLID3";

    static void Main()
    {
        // Retrieve all processes with the specified name.
        var process = Process.GetProcessesByName(PROCESS_NAME);

        // Check if there are no processes with the specified name running.
        if (process.Length == 0)
        {
            Console.WriteLine("Process not found. Make sure METAL GEAR SOLID3 is running.");
            return; // Exit the program.
        }

        // Get the base address of the game process. This address is the starting point of the process in memory.
        IntPtr processBaseAddress = process[0].MainModule.BaseAddress;

        // Display the base address.
        Console.WriteLine($"Base address for {PROCESS_NAME}: {processBaseAddress.ToString("X")}");

        // Loop to keep the program running and ask the user for input.
        while (true)
        {
            Console.WriteLine("Enter the current address you found (or 'q' to quit):");
            string currentAddressStr = Console.ReadLine();

            // If the user types 'q' or 'Q', exit the loop.
            if (currentAddressStr.ToLower() == "q")
            {
                break;
            }

            // Try to parse the input as a hex number. If it fails, display an error message.
            if (string.IsNullOrWhiteSpace(currentAddressStr) || !long.TryParse(currentAddressStr, System.Globalization.NumberStyles.HexNumber, null, out long currentAddress))
            {
                Console.WriteLine("Invalid address.");
                continue; // Go to the next iteration of the loop.
            }

            // Calculate the offset. The offset is the difference between the current address and the base address.
            IntPtr offset = (IntPtr)(currentAddress - (long)processBaseAddress);
            Console.WriteLine($"Offset based on the current address: {offset.ToString("X")}");

            // Recompute the absolute address using the base address and the offset.
            IntPtr computedAddress = IntPtr.Add(processBaseAddress, (int)offset);
            Console.WriteLine($"Recomputed Absolute Address: {computedAddress.ToString("X")}");

            // Display a formatted string to copy and paste into Cheat Engine.
            Console.WriteLine($"For a simple copy and paste into Cheat Engine copy the line below: \n\n{PROCESS_NAME}.exe+{offset.ToString("X")}\n\n");
        }
    }
}