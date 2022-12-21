<h1><img src="https://user-images.githubusercontent.com/72508736/190932358-03bbdeee-375e-46ae-b4e2-eeb103ef4f37.png" width="25"> &nbsp; UNITED HEARTS</h1>

# Lemon Sky

![Image](https://user-images.githubusercontent.com/72508736/190933261-e12d43fb-0240-45a5-bfb5-604d3f639a38.png)


Сетевая-соревновательная игра в средневековом сеттинге, сочетающая в себе некоторые элементы жанра [Battle Royale](https://ru.wikipedia.org/wiki/%D0%9A%D0%BE%D1%80%D0%BE%D0%BB%D0%B5%D0%B2%D1%81%D0%BA%D0%B0%D1%8F_%D0%B1%D0%B8%D1%82%D0%B2%D0%B0_(%D0%B6%D0%B0%D0%BD%D1%80_%D0%BA%D0%BE%D0%BC%D0%BF%D1%8C%D1%8E%D1%82%D0%B5%D1%80%D0%BD%D1%8B%D1%85_%D0%B8%D0%B3%D1%80)) с механиками игры [Fall Guys](https://en.wikipedia.org/wiki/Fall_Guys). 

## Описание игрового процесса

Игра сталкивает на одной карте, некоторое количество игроков, основная цель которых добраться до некоторой точки, чтобы заполучить награду. Игроки сталкиваются с друг другом в процессе пути к цели, либо когда уже добрались до нее. Путь сопровождается головоломками, платформингом, столкновениями с ботами или другими игроками. Путь также содержит награды, которые могут быть интересны для игроков. Игроки могут, как помогать друг другу, так и мешать прохождению.

Основная схема карты реализующей идею представлена ниже

<img src="https://user-images.githubusercontent.com/72508736/190933191-81ab39ed-10b7-4f84-b9ed-df95ac9a48e3.png" width="500">

Игроки Начинают игру из зоны А или В и идут к зоне С, где находится главная награда. Зона А является менее агрессивной и содержит наименьшее количество врагов, но наибольшее количество головоломок и платформинга. Зона В - противоположность зоны А, много врагов, немного головоломок и паркура. Зоны А и В отличаются настроем, как с геймплейной точки зрения, так и с точки зрения дизайна и музакального сопровождения, грубо говоря, зона А - это сказочный лес, зона В - это выжженая земля. Зона С в свою очередь это замок, содержащий более сильных врагов, более сложные головоломки и еще более трудные платформинг зоны.

Для попадания в замок игроки должны, пройти путь до замка не погибнув и справится с головоломкой или паркуром, прохождение которых позволяет игрокам попасть в замок. 

Зоны А и В отделены друг от друга расщелиной идущей от стен замка. В расщелине бушует лава и через нее не получится попасть в противоположную зону. Но расщелина является сложной  платформинг зоной ведущей напрямую внутрь замка.

[Структуру уровня более детально отражает изображение ниже.](https://github.com/BartBurton/Final-Project/blob/master/MainConceptOfMap.drawio)

![Image](https://user-images.githubusercontent.com/72508736/190934203-9e6d3634-2248-4a74-9f20-2adcffefef43.png) 

Зоны не являются полностью открытыми как в играх классическим открытым миром, преимущественно это лабиринты с несколькими открытыми пространствами. Это нужно для того, чтобы решение головоломок и прохождение платформ было обосновано прохождением дальше.

Сценарий сессии игроков следующий:
- Игроки проходят зоны, пытаясь добраться до замка и награды в нем, на это у них есть некоторое количество времени. По истечении времени, если никто не добрался до награды сессия заканчивается - _(Никто не выиграл)_
- Первый игрок, добравшийся до награды, сбрасывает основной таймер сессии и запускает более короткий - _(Никто еще не выиграл)_
- - Когда более короткий таймер запущен, игроки могут голосовать за победу, тех кто дошел до награды или не голосовать.
- - Это голосование определяет финальное противостояние игроков за награду. Таким образом все игроки могут согласится с чьей то победой и битвы не будет, или все могут быть друг против друга, или могут произойти битвы команда на команду, где команды определяются единогласием игроков.
- - Те кто не успеет добраться до награды за время определенное более коротким таймером, проигрывают.
- Игрок/игроки отстоявшие свою позицию относительно награды - выигрывают.
- Победа игроков определяется большим количеством здоровья на момент истечения времени более короткого таймера. Игроки не добравшиеся до награды, но голосовавшие не учитываются при определении победителя.
- - Если уровень здоровья у противников одинаков - никто не выиграл.

Награды получаемые игроком будут являться валютой для покупки внутри игровых предметов.
В качестве предметов для покупки, на первых этапах развития проекта, будут выступать скины персонажей. Информация о количестве внутриигровой валюты игрока, купленных им предметов, статистика провденных в игре сессий хранится в БД на удаленном сервере.

## Референсы и пример геймплея

Источниками вдохновения для создания игры выступают такие произведения как:

| | | | |
|:-------------------------:|:-------------------------:|:-------------------------:|:-------------------------:|
|<img src="https://user-images.githubusercontent.com/72508736/190938575-aea0efed-3192-4ccf-89c4-838081d48098.png" width="200"><br/>[Fall Guys](https://en.wikipedia.org/wiki/Fall_Guys)|<img src="https://user-images.githubusercontent.com/72508736/190938679-1684417c-d9fe-42b7-9106-027e86f53521.png" width="200"><br/>[Fortnite](https://en.wikipedia.org/wiki/Fortnite)|<img src="https://user-images.githubusercontent.com/72508736/190938738-f6c51bb8-dd94-4687-96b8-413d5c56e46c.png" width="200"><br/>[Sly Cooper](https://www.igromania.ru/game/series/1302/Sly_Cooper.html)|<img src="https://user-images.githubusercontent.com/72508736/190938960-d620ce18-fe68-417d-a2cd-5ede174cbac2.png" width="200"><br/>[Medieval](https://ru.wikipedia.org/wiki/MediEvil)|
|<img src="https://user-images.githubusercontent.com/72508736/190939055-7ab3407d-73ee-48e1-a5df-babca5db971c.png" width="200"><br/>[Властелин колец (кинотрилогия)](https://ru.wikipedia.org/wiki/%D0%92%D0%BB%D0%B0%D1%81%D1%82%D0%B5%D0%BB%D0%B8%D0%BD_%D0%BA%D0%BE%D0%BB%D0%B5%D1%86_(%D0%BA%D0%B8%D0%BD%D0%BE%D1%82%D1%80%D0%B8%D0%BB%D0%BE%D0%B3%D0%B8%D1%8F))|<img src="https://user-images.githubusercontent.com/72508736/190939157-3db697c5-836e-402e-9351-d820c11cb9c3.png" width="200"><br/>[SpongeBob SquarePants: Battle for Bikini Bottom – Rehydrated](https://en.wikipedia.org/wiki/SpongeBob_SquarePants:_Battle_for_Bikini_Bottom_%E2%80%93_Rehydrated)|<img src="https://user-images.githubusercontent.com/72508736/190940115-d0d11469-8d6e-44a1-b5c2-03254e1f6e6f.png" width="200"><br/>[Adventure Time](https://en.wikipedia.org/wiki/Adventure_Time)|<img src="https://user-images.githubusercontent.com/72508736/190940248-219af4ec-6baa-4840-ad04-552f95f7b47c.png" width="200"><br/>[Magicka](https://ru.wikipedia.org/wiki/Magicka)|
|<img src="https://user-images.githubusercontent.com/72508736/190940518-4ef1711e-9a2e-4e84-8f87-0289e024aefc.png" width="200"><br/>[World of Warcraft](https://ru.wikipedia.org/wiki/World_of_Warcraft)|<img src="https://user-images.githubusercontent.com/72508736/190940657-a8b1e40a-335e-4684-a410-7659a1cfdf8b.png" width="200"><br/>[Dark Souls](https://ru.wikipedia.org/wiki/Dark_Souls)|<img src="https://user-images.githubusercontent.com/72508736/190940777-bb00026e-8e8c-4663-8018-c7a940fd936c.png" width="200"><br/>[Ведьмак 3: Дикая Охота](https://ru.wikipedia.org/wiki/%D0%92%D0%B5%D0%B4%D1%8C%D0%BC%D0%B0%D0%BA_3:_%D0%94%D0%B8%D0%BA%D0%B0%D1%8F_%D0%9E%D1%85%D0%BE%D1%82%D0%B0)|<img src="https://user-images.githubusercontent.com/72508736/190940860-8bc99ca5-3e71-416e-b4ad-eb78b81dbabf.png" width="200"><br/>[The Legend of Zelda: Breath of the Wild](https://ru.wikipedia.org/wiki/The_Legend_of_Zelda:_Breath_of_the_Wild)|

Также источниками идей дизайна локаций игры служат следующие работы с [ArtStation](https://www.artstation.com):
[Ashes of Creation - Volcano Biome](https://www.artstation.com/artwork/R312NX), [Environment Concept - Fire](https://www.artstation.com/artwork/bKk1Wk), [Skull volcano](https://www.artstation.com/artwork/K8XYG), [Volcano](https://www.artstation.com/artwork/Vw4dg), [Magmar](https://www.artstation.com/artwork/rWVbe), [Magma rocks](https://www.artstation.com/artwork/Vd3588), [Katya Art](https://www.artstation.com/katya_art/albums/86779), [Supernatural City - Tunnel of Love](https://www.artstation.com/artwork/G89zZ4), [30hr quicksketch](https://www.artstation.com/artwork/mgQKa), [Tribal World 12](https://www.artstation.com/artwork/XBZXEy), [Spyro Reignited - Complete Set](https://www.artstation.com/artwork/8ebPQn).

Основная текстура игры похожа на текстуру из класических 2D мультфильмов, т.е. в на основе [Toon Shader](https://www.youtube.com/watch?v=hBztmFHkNQo&t).

В качестве референсов для музыкального сопровождения геймплея и главного меню за основу берутся:
| | | |
|:-------------------------:|:-------------------------:|:-------------------------:|
|<img src="https://user-images.githubusercontent.com/72508736/190942672-190df370-ab98-44b7-a45a-f57b4afe7b6e.png" width="230"><br/>[Skyrim OST](https://www.youtube.com/watch?v=8F1-1j_ZDgc)|<img src="https://user-images.githubusercontent.com/72508736/190942705-078e414f-7ace-4a2d-9ba3-60f5b1543d33.png" width="230"><br/>[Witcher OST](https://www.youtube.com/watch?v=rI2vjPUztJc)|<img src="https://user-images.githubusercontent.com/72508736/190939055-7ab3407d-73ee-48e1-a5df-babca5db971c.png" width="230"><br/>[Lord of the Rings OST](https://www.youtube.com/watch?v=CahOLfYxiq0)|

## Цели

> В первую очередь, должен быть реализован основной игровой процесс игры, [итеративным методом разработки](https://web-creator.ru/articles/iterative_development).
Как только в игре будет реализована основная логика, т.е. можно будет отыграть сессию хотя бы на 2х игроков, можно будет приступать к решению проблем оптимизации, добавления деталей, создания нативного меню игры, корректирования экономики игры, написания более комплексной серверной логики и т.д. На этом этапе можно будет значительно увеличить длительность итераций разработки и при этом ставить на итерации только одну глобальную задачу, например - "Оптимизация сетевого кода."

> Игра должна поддерживать одновременно 16 игроков.
В процессе разработки будет проводится нагрузочное тестирование, для оценки способности программы поддерживать N количество игроков. Начальной целью будет - добиться игры 2х игроков, далее 4х, 8ми и 16ти. При хороших показателях, количество будет увеличиваться.

> Для игры должен быть развернут сервер с базой данных, для хранения данных игроков. 
Необходимо реализовать аутентификацию игроков, с возможностью восстановления аккаунта в случае потери пароля, путем проверки через почту или номер телефона. Сервер должен реализовывать стандартный CRUD для данных нуждающихся в постоянном хранении.

> В игре должно быть нативное меню, выполненное в соответствии со стилем игры, без использования стандартных заготовок.

> Весь контент игры претендующий на интеллектуальную собственность должен быть реализован самостоятельно, либо быть бесплатным для коммерческого использования.

> Игру необходимо выложить на максимально возможное количество площадок по продаже видеоигр.

> Игру должна работать, в первую очередь, на Windows 10/11.

## Инструменты разработки

- В качестве основного инструмента разработки игры выбран игровой движок [Unity3D](https://unity.com/ru), в связке с языком программирования [C#](https://learn.microsoft.com/ru-ru/dotnet/csharp/) и редактором кода [VSCode](https://code.visualstudio.com/). Выбор обоснован наличием умений у команды работать с данными технологиями, наличием удобной и обширной документацией и широким сообществом.

- Для моделирования, анимирования и текстурирования персонажей, предметов и карты используется программа [blender](https://www.blender.org/). Для создания элементов меню и текста будет используется [Photoshop](https://www.adobe.com/ru/products/photoshop.html). Для работы со звуковыми эффектами выбрана программа [Adobe Audition](https://www.adobe.com/ru/products/audition.html).

- Для разработки сервера используется фреймворк [ASP.NET Core](https://dotnet.microsoft.com/en-us/apps/aspnet), и система управления базой данных [MS SQL](https://www.microsoft.com/ru-ru/sql-server/sql-server-2019).

- Управление проектом осуществляется при помощи [Git](https://git-scm.com/), [Github](https://github.com/), [Git Bash](https://git-scm.com/downloads), [GitHub Desctop](https://desktop.github.com/). Управление задач вдется через [GitHub Projects](https://docs.github.com/en/issues/planning-and-tracking-with-projects/creating-projects/creating-a-project)

- Возможно, в процессе работы будут приниматься решения об использовании дополнительного ПО для разработки или вовсе иного.

## Команда 
> Бондарев Богдан – геймплей программист

> Никитин Максим – 3D художник
