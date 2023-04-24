bool playing = true;

List<string> bodyParts = new List<string>() { "head", "neck", "right arm", "left arm", "chest", "torso", "left leg", "right leg" };

while (playing)
{
    Console.WriteLine("\u001b[1mWelcome to Hangman\u001b[0m\n");

    Console.WriteLine("Please enter the amount of letters the word you are guessing should have");
    int numberOfLetters = 0;

    while (!int.TryParse(Console.ReadLine(), out numberOfLetters))
    {
        Console.Write("\u001b[31mThe value must be of integer type, try again:\u001b[0m\n");
    }

    string content = File.ReadAllText("..\\..\\..\\words_alpha.txt");

    List<string> splitList = new List<string>(content.Split("\n"));
    List<Tuple<string, int>> randList = new List<Tuple<string, int>>();

    int index = 0;

    foreach (string word in splitList)
    {
        int wordLength = word.Length - 1;
        index++;

        if (wordLength == numberOfLetters)
        {
            randList.Add(new Tuple<string, int>(word, index));
        }

    }

    if (randList.Count == 0)
    {
        Console.WriteLine("\nNo words with that number of letters exist\n");
        continue;
    }

    Random random = new Random();

    int randomIndex = random.Next(0, randList.Count - 1);
    string randomWord = randList[randomIndex].Item1;
    int randomWordIndex = randList[randomIndex].Item2;
    string revealWord = new string('_', numberOfLetters);

    Console.Clear();

    //Console.WriteLine(randomWord);
    //Console.WriteLine(randomWordIndex + "\n");

    Console.WriteLine($"\nRandom word with {numberOfLetters} letters generated\n");
    Console.WriteLine("\u001b[33mGuess a letter on every turn\n");
    Console.WriteLine("If the guessed letter is in the word:\nThe location of the letter in the word will be revealed\n");
    Console.WriteLine("If the guessed letter is not in the word:\nYou will lose a body part (Hint: you have 8 body parts)\n");
    Console.WriteLine("If you lose all your body parts, by guessing too many letters than aren't in the word, then\nYOU DIE\u001b[0m\n");

    List<int> getAllOccurencesOfLetterInString(char letter, string searchString)
    {
        List<int> indexes = new List<int>();

        int seperateIndex = 0;

        foreach (char letterChar in searchString)
        {
            if (letterChar == letter)
            {
                indexes.Add(seperateIndex);
            }
            seperateIndex++;
        }

        return indexes;
    }

    List<string> guessedLettersList = new List<string>();

    while (bodyParts.Count > 0)
    {
        Console.WriteLine("\u001b[36mPlease enter a guess\u001b[0m");
        string? guessedLetter = Console.ReadLine();

        while (!guessedLetter.Any(Char.IsLetter) || (guessedLetter.Length > 1))
        {
            Console.WriteLine("\u001b[31mPlease enter a valid input\u001b[0m");
            guessedLetter = Console.ReadLine();
        }

        guessedLetter = guessedLetter.ToLower();

        if (randomWord.Contains(guessedLetter))
        {

            if (guessedLettersList.Contains(guessedLetter))
            {
                Console.WriteLine("You have already guessed that letter, please guess again");
                continue;
            }

            guessedLettersList.Add(guessedLetter);

            int correctGuessedLetterIndex = randomWord.IndexOf(guessedLetter);

            List<int> test = new List<int>(getAllOccurencesOfLetterInString(char.Parse(guessedLetter), randomWord));
            foreach (int indextTest in test)
            {
                revealWord = revealWord.Remove(indextTest, 1).Insert(indextTest, guessedLetter);
            }

            Console.WriteLine(revealWord);
        }
        else
        {
            Console.WriteLine("Incorrect\n");

            string removedBodyPart = bodyParts[random.Next(0, bodyParts.Count - 1)];
            bodyParts.Remove(removedBodyPart);

            if (bodyParts.Count < 1)
            {
                Console.WriteLine("Sorry, you lost");
                Console.WriteLine(randomWord);
                playing = false;
                break;
            }

            Console.WriteLine($"You just lost your {removedBodyPart}");
        }

        Console.WriteLine($"You have {bodyParts.Count} limbs remaining\n");

        if (!revealWord.Contains("_"))
        {
            Console.WriteLine("YOU WON!");
            if (!restartGame())
            {
                playing = false;
            }
            break;
        }
    }

    bool restartGame()
    {
        bool confirmed = false;
        bool shouldGameRestart = false;

        Console.WriteLine("Would you like to play again?");

        string? playAgainInput = Console.ReadLine();

        do
        {    
            if (playAgainInput == "Yes")
            {
                shouldGameRestart = true;
                confirmed = true;
            } else if (playAgainInput == "No")
            {
                shouldGameRestart = false;

                confirmed = true;
            }
            else
            {
                Console.WriteLine("Please enter \"Yes\" or \"No\"?");
                playAgainInput = Console.ReadLine();
            }
        } while (!confirmed);

        return shouldGameRestart;
    }
}