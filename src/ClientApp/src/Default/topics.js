
const addSections = ()=>{
    createSection("C++", "C++ is a general-purpose programming language. It was originally designed as an extension to C and has a similar syntax, but it is now a completely different language. Use this tag for questions about code (to be) compiled with a C++ compiler.");
    createSection("Java", "Java is a high-level object-oriented programming language. Use this tag when you're having problems using or understanding the language itself. This tag is frequently used alongside other tags for libraries and/or frameworks used by Java developers.");
    createSection("C", "C is a general-purpose programming language used for system programming (OS and embedded), libraries, games and cross-platform. This tag should be used with general questions concerning the C language, as defined in the ISO 9899 standard (the latest version, 9899:2018, unless otherwise specified — also tag version-specific requests with c89, c99, c11, etc). C is distinct from C++ and it should not be combined with the C++ tag without a specific reason.");
    createSection("Ruby on Rails", "The best ruby framework. RoR");
    createSection("Java Spring", "Spring makes programming Java quicker, easier, and safer for everybody. Spring’s focus on speed, simplicity, and productivity has made it the world's most popular Java framework.");
    createSection("Haskell", "Haskellers interact, talk and collaborate across several mediums and around the world. There are places to learn, to teach, to ask questions, and to find contributors and collaborators.");
    createSection("Python", "Python is a programming language that lets you work quickly and integrate systems more effectively.");
    createSection("Front-end", "Front-end web development is the development of the graphical user interface of a website, through the use of HTML, CSS, and JavaScript, so that users can view and interact with that website");
    createSection("C#", "C# (pronounced 'see sharp') is a high level, statically typed, multi-paradigm programming language developed by Microsoft. C# code usually targets Microsoft's .NET family of tools and run-times, which include .NET, .NET Framework, .NET MAUI, and Xamarin among others. Use this tag for questions about code written in C# or about C#'s formal specification.");
    createSection("Golang", "At the time, no single team member knew Go, but within a month, everyone was writing in Go and we were building out the endpoints. It was the flexibility, how easy it was to use, and the really cool concept behind Go (how Go handles native concurrency, garbage collection, and of course safety+speed.) that helped engage us during the build. Also, who can beat that cute mascot!");
    createSection("OOP", "Object-oriented programming (OOP) is a computer programming model that organizes software design around data, or objects, rather than functions and logic. An object can be defined as a data field that has unique attributes and behavior.");

}
const addUsers = ()=>{
    const cpp =1, java=9,csharp=3, go=5, py=11;
    register("marko82", "password").then((result) => {
        authenticate("marko82", "password").then(res=>{
            addCppTopics(cpp, 1);
            addCppTopics(cpp, 2);
            addPythonTopics(py, 1);
        });
    });
    register("megarekrut65", "password").then((result) => {
        authenticate("megarekrut65", "password").then(res=>{
            addCppTopics(cpp, 3);
            addPythonTopics(py, 2);
            addCSharpTopics(csharp, 1);
            addGoTopics(go, 1);
            addJavaTopics(java, 1);
        });
    });
    register("karlson3000", "password").then((result) => {
        authenticate("karlson3000", "password").then(res=>{
            addPythonTopics(py, 3);
            addCSharpTopics(csharp, 2);
            addGoTopics(go, 2);
        });
    });
    register("mike anderson", "password").then((result) => {
        authenticate("mike anderson", "password").then(res=>{
            addCSharpTopics(csharp, 3);
            addGoTopics(go, 3);
            addGoTopics(go, 4);
        });
    });
    register("Kornelius", "password").then((result) => {
        authenticate("Kornelius", "password").then(res=>{
            addJavaTopics(java, 2);
        });
    });
}
const addCppTopics = (id, index)=>{
    switch(index){

    case 1:
    createTopic(id, 
        "Can't get sum of array inside array", 
        "The sum of the following array does not show correctly, instead it shows large junk of digits. What I did wrong.", 
        `
        #include <cmath>
        #include <cstdio>
        #include <vector>
        #include <iostream>
        #include <algorithm>
        #include <string>
        using namespace std;
        
        //classes
        //classes
        class Person{
        public:
            virtual void getdata() = 0;
            virtual void putdata() = 0;
        protected:
            string name;
            int age;
        };
        
        class Professor : public Person{
        public:
            Professor() {
            }
            void getdata(){
                cin >> name;
                cin >> age >> publications;
                cur_id++;
                no_of++;
            }
            void putdata(){
                cout << name << " ";
                cout << age << " ";
                cout << publications << " ";
                cout <<cur_id - no_of<< endl;
                no_of--;
            }
        private:
            int publications;
            static int cur_id;
            static int no_of;
        };
        
        class Student : public Person{
        public:
            Student(){
            }
            void getdata(){
                cin >> name >> age;
                for(int i=0;i<6;i++){
                    cin >> marks[i];
                }
                cur_id++;
                no_of++;
            }
            void putdata(){
                int mark_sum;
                cout << name << " " << age << " ";
                for(int i=0;i<6;i++){
                    mark_sum += marks[i];
                }
                cout << mark_sum << " ";
                cout <<cur_id - no_of<< endl;
                no_of--;
            }
        private:
            int marks[6];
            static int cur_id;
            static int no_of;
        };
        
        //static variables
        int Student::cur_id = 0;
        int Professor::cur_id = 0;
        int Student::no_of = -1;
        int Professor::no_of = -1;
        
        //main
        int main(){
        
            int n, val;
            cin>>n; //The number of objects that is going to be created.
            Person *per[n];
        
            for(int i = 0;i < n;i++){
        
                cin>>val;
                if(val == 1){
                    // If val is 1 current object is of type Professor
                    per[i] = new Professor;
        
                }
                else per[i] = new Student; // Else the current object is of type Student
        
                per[i]->getdata(); // Get the data from the user.
        
            }
        
            for(int i=0;i<n;i++)
                per[i]->putdata(); // Print the required output for each object.`, 
        "cpp");
        break;
        case 2:
    createTopic(id, 
        "How to generate a random number in C++?",
        "I'm trying to make a game with dice, and I need to have random numbers in it (to simulate the sides of the die. I know how to make it between 1 and 6). Using",
        `
        #include <cstdlib> 
        #include <ctime> 
        #include <iostream>
        
        using namespace std;
        
        int main() 
        { 
            srand((unsigned)time(0)); 
            int i;
            i = (rand()%6)+1; 
            cout << i << "\\n"; 
        }`,
        "cpp");
        break;
        case 3:
    createTopic(id,
        "Print array in visual c++",
        "Hi I need print my array string but I don't know this is the code the array is on string and I need print all position",
        `#include "stdafx.h"
        #include <iostream>
        using namespace std ;
        
        int main()
        {
            string jmena[8]; 
        
            //populate array:
            jmena[0] = "Ruzicka Vit";
            jmena[1] = "Bily Saruman";
            jmena[2] = "Veliky Saruman";
            jmena[3] = "Calculator Veliky";
            jmena[4] = "Jakekoliv Jmeno";
            jmena[5] = "Nekdo Veliky";
            jmena[6] = "Ahoj Vitek";
            jmena[7] = "Whatever Name";
        
            int hola = 0;     
        
            cout << "Nombres " << endl;
        
            for (int i = 0; i < 5 ; i++) {
        
                cout << jmena[i] ; // I have error here IntelliSense: no operator "<<" matches these operands
        
            }
            cin >> hola;
        
        }`,
        "cpp");
        break;}
}
const addJavaTopics = (id, index)=>{
    switch(index){

    case 1:
    createTopic(id,
        "Reading float from console",
        "How can i get float number from console? I can get only integer(",
        `
        import java.util.Scanner;

        public class HelloWorld {

            public static void main(String[] args) {

                // Creates a reader instance which takes
                // input from standard input - keyboard
                Scanner reader = new Scanner(System.in);
                System.out.print("Enter a number: ");

                // nextInt() reads the next integer from the keyboard
                int number = reader.nextInt();

                // println() prints the following line to the output screen
                System.out.println("You entered: " + number);
            }
        }`,
        "java"
    );
    break;
    case 2:
    createTopic(id,
        "Checking number",
        "Check whether a number is even or odd using if...else statement",
        `
        import java.util.Scanner;

        public class EvenOdd {

            public static void main(String[] args) {

                Scanner reader = new Scanner(System.in);

                System.out.print("Enter a number: ");
                int num = reader.nextInt();

                if(num % 2 == 0)
                    System.out.println(num + " is even");
                else
                    System.out.println(num + " is odd");
            }
        }`,
        "java");
        break;
    }
}
const addGoTopics = (id, index)=>{
    switch(index){

    case 1:
    createTopic(id,
        "Begin lesson",
        "Try this to start learn Go",
        `
        // You can edit this code!
        // Click here and start typing.
        package main
        
        import "fmt"
        
        func main() {
            fmt.Println("Hello, 世界")
        }`,
        "go"
    );
    break;
    case 2:
    createTopic(id,
        "Concurrent in Go",
        "Try this to learn Go",
        `
        // Concurrent computation of pi.
        // The implementation uses the Nilakantha Series.
        //
        // This demonstrates Go's ability to handle
        // large numbers of concurrent processes.
        // It is an unreasonable way to calculate pi.
        package main

        import (
            "fmt"
            "math"
        )

        func main() {
            fmt.Println("          math.Pi:", math.Pi)
            fmt.Println("Nilakantha Series:", pi(5000))
        }
        
        // pi launches n goroutines to compute an
        // approximation of pi.
        func pi(n int) float64 {
            ch := make(chan float64)
            for k := 0; k < n; k++ {
                go term(ch, float64(k))
            }
            f := 3.0
            for k := 0; k < n; k++ {
                f += <-ch
            }
            return f
        }
        
        func term(ch chan float64, k float64) {
            ch <- 4 * math.Pow(-1, k) / ((2*k + 2) * (2*k + 3) * (2*k + 4))
        }
        `,
        "go"
    );
    break;
    case 3:
    createTopic(id,
        "Fibonacci in Go",
        "There is the best way to find fibonacci numbers",
        `
        import "fmt"

        // fib returns a function that returns
        // successive Fibonacci numbers.
        func fib() func() int {
            a, b := 0, 1
            return func() int {
                a, b = b, a+b
                return a
            }
        }

        func main() {
            f := fib()
            // Function calls are evaluated left-to-right.
            fmt.Println(f(), f(), f(), f(), f())
        }`,
        "go"
    );
    break;
    case 4:
    createTopic(id,
        "Piano printing",
        "Try this to learn Go",
        `
        // Peano integers are represented by a linked
        // list whose nodes contain no data
        // (the nodes are the data).
        // http://en.wikipedia.org/wiki/Peano_axioms

        // This program demonstrates that Go's automatic
        // stack management can handle heavily recursive
        // computations.

        package main

        import "fmt"

        // Number is a pointer to a Number
        type Number *Number

        // The arithmetic value of a Number is the
        // count of the nodes comprising the list.
        // (See the count function below.)

        // -------------------------------------
        // Peano primitives

        func zero() *Number {
            return nil
        }

        func isZero(x *Number) bool {
            return x == nil
        }

        func add1(x *Number) *Number {
            e := new(Number)
            *e = x
            return e
        }

        func sub1(x *Number) *Number {
            return *x
        }

        func add(x, y *Number) *Number {
            if isZero(y) {
                return x
            }
            return add(add1(x), sub1(y))
        }

        func mul(x, y *Number) *Number {
            if isZero(x) || isZero(y) {
                return zero()
            }
            return add(mul(x, sub1(y)), x)
        }

        func fact(n *Number) *Number {
            if isZero(n) {
                return add1(zero())
            }
            return mul(fact(sub1(n)), n)
        }

        // -------------------------------------
        // Helpers to generate/count Peano integers

        func gen(n int) *Number {
            if n > 0 {
                return add1(gen(n - 1))
            }
            return zero()
        }

        func count(x *Number) int {
            if isZero(x) {
                return 0
            }
            return count(sub1(x)) + 1
        }

        // -------------------------------------
        // Print i! for i in [0,9]

        func main() {
            for i := 0; i <= 9; i++ {
                f := count(fact(gen(i)))
                fmt.Println(i, "! =", f)
            }
        }`,
        "go");
        break;
    }

}
const addCSharpTopics = (id, index)=>{
    switch(index){

    case 1:
    createTopic(id,
        "Print string in console",
        "There is printing name to console",
        `
        using System;

        namespace MyApplication
        {
        class Program
        {
            static void Main(string[] args)
            {
            string name = "John";
            Console.WriteLine("Hello " + name);
            }
        }
        }`,
        "csharp"
    );
    break;
    case 2:
    createTopic(id,
        "Double to int",
        "Why float part of number is hidden in int?",
        `
        using System;

        namespace MyApplication
        {
        class Program
        {
            static void Main(string[] args)
            {
            double myDouble = 9.78;
            int myInt = (int) myDouble;  // Manual casting: double to int

            Console.WriteLine(myDouble);
            Console.WriteLine(myInt);
            }
        }
        }`,
        "csharp"
    );
    break;
    case 3:
    createTopic(id,
        "Sqrt in c#",
        "How it works? Can't someone explain?",
        `
        using System;

        namespace MyApplication
        {
        class Program
        {
            static void Main(string[] args)
            {
                Console.WriteLine(Math.Sqrt(64));  
            }
        }
        }`,
        "csharp"
    );
    break;
}
};

const addPythonTopics = (id, index)=>{
    switch(index){

    case 1:
    createTopic(id,
        "Swapping in python",
        "It's the best way to swap to numbers in python?",
        `
        # Python program to swap two variables

        x = 5
        y = 10

        # To take inputs from the user
        #x = input('Enter value of x: ')
        #y = input('Enter value of y: ')

        # create a temporary variable and swap the values
        temp = x
        x = y
        y = temp

        print('The value of x after swapping: {}'.format(x))
        print('The value of y after swapping: {}'.format(y))`,
        "python3"
    );
    break;
    case 2:
    createTopic(id,
        "Matrices in Py",
        "Source code: Matrix Addition using Nested Loop",
        `
        # Program to add two matrices using nested loop

        X = [[12,7,3],
            [4 ,5,6],
            [7 ,8,9]]

        Y = [[5,8,1],
            [6,7,3],
            [4,5,9]]

        result = [[0,0,0],
                [0,0,0],
                [0,0,0]]

        # iterate through rows
        for i in range(len(X)):
        # iterate through columns
        for j in range(len(X[0])):
            result[i][j] = X[i][j] + Y[i][j]

        for r in result:
        print(r)`,
        "python3"
    );
    break;
    case 3:
    createTopic(id,
        "Multiplication and loops",
        "What is wrong there? I want to get multiplication from 1 to 10 but get from 1 to 11",
        `
        # Multiplication table (from 1 to 10) in Python

        num = 12

        # To take input from the user
        # num = int(input("Display multiplication table of? "))

        for i in range(1, 12):
        print(num, 'x', i, '=', num*i)`,
        "python3"
    );
    break;}
};