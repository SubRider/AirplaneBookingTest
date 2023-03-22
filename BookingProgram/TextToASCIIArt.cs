static class ToASCIIArt
{
    public static void Write(string characters, int linesAbove = 0)
    {
        for(int i = 0; i < 10; i++)
        {
            int j = i + linesAbove * 11;
            int totalWidth = 0;
            foreach (char character in characters)
            {
                try
                {
                    Console.SetCursorPosition(totalWidth, j);
                    string roman = CharToRoman(character);
                    int width = (roman.Length / 10);
                    totalWidth += width + 1;
                    Console.Write(roman.Substring(i * width, (width - 1)));
                }
                catch (ArgumentOutOfRangeException)
                {
                    continue;
                }   
            }
        }
        Console.SetCursorPosition(0, linesAbove * 10 + 11);
    }

    private static string CharToRoman(char character)
    {
        switch (character)
        {
            case 'a':
                return "                     .oooo.   `P  )88b   .oP\"888  d8(  888  `Y888\"\"8o                               ";
            case 'b':
                return " .o8       \"888        888oooo.   d88' `88b  888   888  888   888  `Y8bod8P'                                  ";
            case 'c':
                return "                     .ooooo.  d88' `\"Y8 888       888   .o8 `Y8bod8P'                               ";
            case 'd':
                return "      .o8       \"888   .oooo888  d88' `888  888   888  888   888  `Y8bod88P\"                                  ";
            case 'e':
                return "                     .ooooo.  d88' `88b 888ooo888 888    .o `Y8bod8P'                               ";
            case 'f':
                return " .o88o.  888 `\" o888oo   888     888     888    o888o                           ";
            case 'g':
                return "                       .oooooooo 888' `88b  888   888  `88bod8P'  `8oooooo.  d\"     YD  \"Y88888P'             ";
            case 'h':
                return "oooo        `888         888 .oo.    888P\"Y88b   888   888   888   888  o888o o888o                                     ";
            case 'i':
                return " o8o   `\"'  oooo  `888   888   888  o888o                   ";
            case 'j':
                return "    o8o     `\"'    oooo    `888     888     888     888     888 .o. 88P `Y888P  ";
            case 'k':
                return "oooo        `888         888  oooo   888 .8P'    888888.     888 `88b.  o888o o888o                                     ";
            case 'l':
                return "oooo  `888   888   888   888   888  o888o                   ";
            case 'm':
                return "                                    ooo. .oo.  .oo.   `888P\"Y88bP\"Y88b   888   888   888   888   888   888  o888o o888o o888o                                                       ";
            case 'n':
                return "                        ooo. .oo.   `888P\"Y88b   888   888   888   888  o888o o888o                                     ";
            case 'o':
                return "                     .ooooo.  d88' `88b 888   888 888   888 `Y8bod8P'                               ";
            case 'p':
                return "                      oo.ooooo.   888' `88b  888   888  888   888  888bod8P'  888       o888o                 ";
            case 'q':
                return "                       .ooooo oo d88' `888  888   888  888   888  `V8bod888        888.       8P'        \"    ";
            case 'r':
                return "                  oooo d8b `888\"\"8P  888      888     d888b                               ";
            case 's':
                return "                   .oooo.o d88(  \"8 `\"Y88b.  o.  )88b 8\"\"888P'                            ";
            case 't':
                return "    .     .o8   .o888oo   888     888     888 .   \"888\"                         ";
            case 'u':
                return "                        oooo  oooo  `888  `888   888   888   888   888   `V88V\"V8P'                                     ";
            case 'v':
                return "                        oooo    ooo  `88.  .8'    `88..8'      `888'        `8'                                         ";
            case 'w':
                return "                                  oooo oooo    ooo  `88. `88.  .8'    `88..]88..8'      `888'`888'        `8'  `8'                                                        ";
            case 'x':
                return "                        oooo    ooo  `88b..8P'     Y888'     .o8\"'88b   o88'   888o                                     ";
            case 'y':
                return "                        oooo    ooo  `88.  .8'    `88..8'      `888'        .8'     .o..P'      `Y8P'                   ";
            case 'z':
                return "                        oooooooo  d'\"\"7d8P     .d8P'    .d8P'  .P d8888888P                                   ";
            case 'A':
                return "      .o.            .888.          .8\"888.        .8' `888.      .88ooo8888.    .8'     `888.  o88o     o8888o                                                 ";
            case 'B':
                return "oooooooooo.  `888'   `Y8b  888     888  888oooo888'  888    `88b  888    .88P o888bood8P'                                         ";
            case 'C':
                return "  .oooooo.    d8P'  `Y8b  888          888          888          `88b    ooo   `Y8bood8P'                                         ";
            case 'D':
                return "oooooooooo.   `888'   `Y8b   888      888  888      888  888      888  888     d88' o888bood8P'                                             ";
            case 'E':
                return "oooooooooooo `888'     `8  888          888oooo8     888    \"     888       o o888ooooood8                                        ";
            case 'F':
                return "oooooooooooo `888'     `8  888          888oooo8     888    \"     888         o888o                                               ";
            case 'G':
                return "  .oooooo.     d8P'  `Y8b   888           888           888     ooooo `88.    .88'   `Y8bood8P'                                             ";
            case 'H':
                return "ooooo   ooooo `888'   `888'  888     888   888ooooo888   888     888   888     888  o888o   o888o                                           ";
            case 'I':
                return "ooooo `888'  888   888   888   888  o888o                   ";
            case 'J':
                return "   oooo    `888     888     888     888     888 .o. 88P `Y888P                  ";
            case 'K':
                return "oooo    oooo `888   .8P'   888  d8'     88888[       888`88b.     888  `88b.  o888o  o888o                                        ";
            case 'L':
                return "ooooo        `888'         888          888          888          888       o o888ooooood8                                        ";
            case 'M':
                return "ooo        ooooo `88.       .888'  888b     d'888   8 Y88. .P  888   8  `888'   888   8    Y     888  o8o        o888o                                                    ";
            case 'N':
                return "ooooo      ooo `888b.     `8'  8 `88b.    8   8   `88b.  8   8     `88b.8   8       `888  o8o        `8                                               ";
            case 'O':
                return "  .oooooo.    d8P'  `Y8b  888      888 888      888 888      888 `88b    d88'  `Y8bood8P'                                         ";
            case 'P':
                return "ooooooooo.   `888   `Y88.  888   .d88'  888ooo88P'   888          888         o888o                                               ";
            case 'Q':
                return "  .oooooo.       d8P'  `Y8b     888      888    888      888    888      888    `88b    d88b     `Y8bood8P'Ybd'                                                 ";
            case 'R':
                return "ooooooooo.   `888   `Y88.  888   .d88'  888ooo88P'   888`88b.     888  `88b.  o888o  o888o                                        ";
            case 'S':
                return " .oooooo..o d8P'    `Y8 Y88bo.       `\"Y8888o.       `\"Y88b oo     .d8P 8\"\"88888P'                                      ";
            case 'T':
                return "ooooooooooooo 8'   888   `8      888           888           888           888          o888o                                               ";
            case 'U':
                return "ooooo     ooo `888'     `8'  888       8   888       8   888       8   `88.    .8'     `YbodP'                                              ";
            case 'V':
                return "oooooo     oooo  `888.     .8'    `888.   .8'      `888. .8'        `888.8'          `888'            `8'                                                       ";
            case 'W':
                return "oooooo   oooooo     oooo  `888.    `888.     .8'    `888.   .8888.   .8'      `888  .8'`888. .8'        `888.8'  `888.8'          `888'    `888'            `8'      `8'                                                                                  ";
            case 'X':
                return "ooooooo  ooooo  `8888    d8'     Y888..8P        `8888'        .8PY888.      d8'  `888b   o888o  o88888o                                              ";
            case 'Y':
                return "oooooo   oooo  `888.   .8'    `888. .8'      `888.8'        `888'          888          o888o                                               ";
            case 'Z':
                return " oooooooooooo d'\"\"\"\"\"\"d888'       .888P        d888'       .888P        d888'    .P .8888888888P                                            ";
            case '\'':
                return "o8o `YP  '                              ";
            case '.':
                return "                    .o. Y8P             ";
            case '&':
                return "  .oo.     .88' `8.   88.  .8'   `88.8P      d888[.8'  88' `88.   `bodP'`88.                                  ";
            case '*':
                return "   o    `8.8.8' .8'8`8.    \"                                                    ";
            case '-':
                return "                                8888888                                         ";
            case '_':
                return "                                                                        ooooooooooo                                     ";
            case '(':
                return "  .o  .8' .8'  88   88   `8.   `8.   `\"           ";
            case ')':
                return "o.   `8.   `8.   88   88  .8' .8'  \"'             ";
            case ',':
                return "                    .o. Y8P  '          ";
            case ' ':
                return "          ";
            case '1':
                return "  .o  o888   888   888   888   888  o888o                   ";
            case '2':
                return "  .oooo.   .dP\"\"Y88b        ]8P'     .d8P'    .dP'     .oP     .o 8888888888                                  ";
            case '3':
                return "  .oooo.   .dP\"\"Y88b        ]8P'     <88b.       `88b. o.   .88P  `8bd88P'                                    ";
            case '4':
                return "      .o       .d88     .d'888   .d'  888   88ooo888oo      888       o888o                                   ";
            case '5':
                return "  oooooooo  dP\"\"\"\"\"\"\" d88888b.       `Y88b        ]88  o.   .88P  `8bd88P'                                    ";
            case '6':
                return "    .ooo     .88'      d88'      d888P\"Ybo. Y88[   ]88 `Y88   88P  `88bod8'                                   ";
            case '7':
                return " ooooooooo d\"\"\"\"\"\"\"8'       .8'       .8'       .8'       .8'       .8'                                       ";
            case '8':
                return " .ooooo.   d88'   `8. Y88..  .8'  `88888b.  .8'  ``88b `8.   .88P  `boood8'                                   ";
            case '9':
                return " .ooooo.   888' `Y88. 888    888  `Vbood888       888'     .88P'    .oP'                                      ";
            case '0':
                return "  .oooo.    d8P'`Y8b  888    888 888    888 888    888 `88b  d88'  `Y8bd8P'                                   ";
            default:
                return "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
        }
    }
}