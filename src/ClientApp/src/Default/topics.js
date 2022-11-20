
const addSections = ()=>{
    createSection("C++", "C++ is a general-purpose programming language. It was originally designed as an extension to C and has a similar syntax, but it is now a completely different language. Use this tag for questions about code (to be) compiled with a C++ compiler.");
    createSection("Java", "Java is a high-level object-oriented programming language. Use this tag when you're having problems using or understanding the language itself. This tag is frequently used alongside other tags for libraries and/or frameworks used by Java developers.");
    createSection("C", "C is a general-purpose programming language used for system programming (OS and embedded), libraries, games and cross-platform. This tag should be used with general questions concerning the C language, as defined in the ISO 9899 standard (the latest version, 9899:2018, unless otherwise specified — also tag version-specific requests with c89, c99, c11, etc). C is distinct from C++ and it should not be combined with the C++ tag without a specific reason.");
    createSection("Ruby on Rails", "The best ruby framework. RoR");
}
const addTopics = ()=>{
    createTopic(1, 
        "Can't get sum of array inside array", 
        "The sum of the following array does not show correctly, instead it shows large junk of digits. What I did wrong.", 
        `#include <cmath>
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
    createTopic(1, 
        "How to generate a random number in C++?",
        "I'm trying to make a game with dice, and I need to have random numbers in it (to simulate the sides of the die. I know how to make it between 1 and 6). Using",
        `#include <cstdlib> 
        #include <ctime> 
        #include <iostream>
        
        using namespace std;
        
        int main() 
        { 
            srand((unsigned)time(0)); 
            int i;
            i = (rand()%6)+1; 
            cout << i << "\n"; 
        }`,
        "cpp");
    createTopic(1,
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
}