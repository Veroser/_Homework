#include<iostream> 
#include<vector>
using namespace std;
void main(){ 
    vector<Ability*> abilities;
    abilities.push_back(new Fireball());
    abilities.push_back(new Healing());
    abilities.push_back(new Shield());
    
    for(int i = 0; i < abilities.size(); i++){
        abilities[i]->use();
    }  
}
class Ability{
public: 
    virtual void use() = 0;
    virtual ~Ability()  {}
};

class Fireball : public Ability{
public: 
    void use(){
        cout << "Нанесён урон огнём" << endl;
    }
};

class Healing : public Ability{
public: 
    void use(){
        cout << "Восстановлено здоровье" << endl;
    }
};

class Shield : public Ability{
public: 
    void use(){
        cout << "Активирован щит" << endl;
    }
};