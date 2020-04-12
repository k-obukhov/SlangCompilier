module Main

start
    const integer length := 4;
    array [length] integer myArr;

    variable integer i := 0;
    while (i < length) do
        let myArr[i] := i;
        let i := i + 1;
    end

    let i := 0;
    while (i < length) do
        output i, '\n';
        let i := i + 1;
    end

    array [length][length] integer demoArr;
    let i := 0;
    variable integer j := 0;
    while (i < length) do
        let j := 0;
        while (j < length) do
            let demoArr[i][j] := i * length + j;
            let j := j + 1;
        end
        let i := i + 1;
    end

    let i := 0;
    while (i < length) do
        let j := 0;
        while (j < length) do
            output demoArr[i][j];
            let j := j + 1;
        end
        let i := i + 1;
    end
end