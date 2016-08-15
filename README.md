# Jak powstają dendrogramy

**Dendrogram** to rodzaj wykresu, który pozwala na łączenie w grupy podobnych do siebie elementów. Dzięki drzewiastej strukturze możemy zaobserwować, jaki poziom szczegółowości daje optymalny efekt (im mniej grup, tym będą bardziej ogólne).

Program pozwala na zilustrowanie procesu powstawania dendrogramów na przykładzie zbioru liczb. Wczytujemy plik zawierający różne liczby i krok po kroku widzimy, jak algorytm znajduje "najbliższe sobie" liczby a następnie łączy je w grupy. Są też do wyboru 4 metody ustalania "podobieństwa" między grupami, więc można zobaczyć jaki to ma wpływ na wynik.

* Technologie: **.NET/C#, WPF, XAML**.
* Wzorce projektowe: **MVVM, Command**.
