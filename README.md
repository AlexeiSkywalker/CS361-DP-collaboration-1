# CS361-DP-collaboration-1
## DreamTeam
--------------------------------------------------------------------------------
## Лабораторная 2
## Вариант №2
## Шифрование
================================================================================
--------------------------------------------------------------------------------
### Описание классов.

public class CryptorAES : ICrypto - реализует алгоритм симетричного шифрования AES

public class CryptorDES : ICrypto - реализует алгоритм симетричного шифрования DES

public class ParallelCipher - Реализует шифрование текста с помощью 'n' "рабочих".

public class FileCryptor - Реализует шифрование файлов при помощи класса ParallelCipher.

### UML-диаграмму классов.

![alt text](https://github.com/AlexeiSkywalker/CS361-DP-collaboration-1/blob/master/DreamTeamL2V2/ClassDisgram.png "ClassDisgram")

### Описание использованных порождающих паттернов.

Паттерн Abstract Factory

Абстрактная фабрика - паттерн, порождающий объекты.
Назначение:
Предоставляет интерфейс для создания семейств взаимосвязанных или взаи-
мозависимых объектов, не специфицируя их конкретных классов.
Применимость
Используйте паттерн абстрактная фабрика, когда:
Q система не должна зависеть от того, как создаются, компонуются и пред-
ставляются входящие в нее объекты;
а входящие в семейство взаимосвязанные объекты должны использоваться
вместе и вам необходимо обеспечить выполнение этого ограничения;
а система должна конфигурироваться одним из семейств составляющих ее
объектов;
а вы хотите предоставить библиотеку объектов, раскрывая только их интер-
фейсы, но не реализацию.


### Описание вклада каждого разработчика
Алексей Паращевин: архитектура проекта
Рязанова Анна: интерфейс
Завгороднев Евгений: ментальная связь с командой
