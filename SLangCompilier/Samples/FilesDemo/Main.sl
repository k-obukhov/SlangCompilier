import Files
module Main
start
    variable Files.File myFile := Files.Open("myFile.txt");

    // write
    call Files.WriteInteger(myFile, 1234);
    call Files.WriteCharacter(myFile, ' '); // delimeter
    call Files.WriteReal(myFile, 12.123);
    call Files.WriteCharacter(myFile, ' ');  // delimeter
    call Files.WriteString(myFile, "HelloWorld!");
    call Files.WriteCharacter(myFile, ' ');  // delimeter
    call Files.WriteBoolean(myFile, true);
    call Files.WriteCharacter(myFile, ' ');  // delimeter
    call Files.WriteCharacter(myFile, 'f');
    call Files.Close(myFile);

    // read
    let myFile := Files.Open("myFile.txt");
    output Files.ReadInteger(myFile), '\n';
    output Files.ReadReal(myFile), '\n';
    output Files.ReadString(myFile), '\n';
    output Files.ReadBoolean(myFile), '\n';
    output Files.ReadCharacter(myFile), '\n';
    output "Is open? = ", Files.IsOpen(myFile), '\n';
    output "Is EOF? = ", Files.IsEOF(myFile), '\n';
    call Files.Close(myFile);
    
end