# Lab.ExchangeNet45
É um laboratório com 3 aplicações simples:

1. Web API: É um "serviço de exchange" responsável por expor operações bastante elementares de exchange.
2. Desktop App: É uma aplicação desktop read-only, um viewer que consulta o "serviço de exchange" e apresenta os dados em uma grid excel-like de forma "integral" ou agrupada.
3. Operação Loader: É uma aplicação que importa uma massa de dados do arquivo JSON preexistente na raiz do projeto para o resource `/operacoes` exposto pelo "serviço de exchange". A massa preexistente possui 20K registros.



## Modo de Uso pelo Visual Studio 2019

Dado que o solution está carregado e que todos os NuGet packages foram restored:

- Faça com que o solution execute múltiplos projetos:
  - Ir em properties do solution, no menu que abrir, no lado esquerdo opção "Common Properties > Startup Project".
  - Marcar "Multiple startup projects" em *Lab.ExchangeNet45.WebApi*, *Lab.ExchangeNet45.OperacaoLoader* e *Lab.ExchangeNet45.DesktopApp*.
- Agora é só mandar executar (apertar F5).

Execução esperada:

- O *Web API* irá abrir no browser o Swagger UI;
- O *Operação Loader* um console com 2 opções de carga de dados, sendo que para selecionar a opção padrão, basta pressionar Enter que a carga será feita imediatamente;
- E o *Desktop App* com as abas para visualizar os dados, cada aba com opções para visualizar na hora ou baixar em arquivo CSV ou Excel.



## Overview da Solução

Na raiz do solution, é possível notar 7 projects no total, 5 dentro do folder "ExchangeService" e 2 na raiz. 

> OBS.: As vezes não é preciso ter esse nível tão granular de segregação de projects, mas por ser uma aplicação de laboratório...

E dado que há também outros caminhos que chegam no mesmo objetivo, o raciocínio utilizado foi o seguinte: 

- O folder *ExchangeService* representa o serviço (business capability), enquanto que os 2 projects que estão de fora representam os consumidores desse serviço.
  - O project *Application* é um class library que condensa os <u>modelos de domínio</u>, <u>persistência</u> e <u>queries</u>. No entanto, os namespaces segregam as operações de escrita (command side) das operações de leitura (query side). 
  - O project *WebApi*, que se "pluga" no *Application*, então expõe na rede* as operações de exchange que podem ser performadas via HTTP, servindo como porta de entrada (inbound) para o *Application*. O Swagger UI ajuda a descrever as rotas expostas, content types suportados, e formato de mensagem (no caso o body dos endpoints que usam POST).
  - O project *Contracts* representa o contrato aceito pelo *Application*. É um class library com DTOs que possuem nomenclatura para representar além de conteúdo, a operação que performa. Também poderia ser entendido como *mensagens*. Essas *mensagens* estão subdivididas em *commands* e *queries*. No fim das contas, o objetivo é a mensagem ser entregue e processada pelo *Application*, e o *WebApi* torna isso possível via HTTP.
- O project *OperacaoLoader* é bastante simples. Ele lê um arquivo JSON e faz POST dos registros lidos no *WebApi*. Isso quer dizer que, não foi utilizado um critério estrito de *software design* para implementar o que ele precisa fazer.
- O project *DesktopApp* também é simples. É um WPF com MVVM, onde a ViewModel principal é composta pelas ViewModels que fazem as devidas consultas através no *WebApi*. Cada ViewModel é uma consulta diferente, resultando em grids distintas. As grids são excel-like com opção para consultar em tela, ou extrair arquivo CSV e Excel.



## Persistência

A persistência configurada no project *Application* utiliza o NHibernate com o SQLite.

Na primeira execução, um arquivo SQLite será automaticamente criado com toda a estrutura necessária no diretório *%USERPROFILE%\AppData\Local\Temp* com o nome *Lab.ExchangeNet45.sqlite*. Veja detalhes na classe `NHibernateSessionFactoryFactory.cs`.



## Cache Web API

O cache utilizado é um cache simples 3rd party com duração de 3 minutos in-memory. A vantagem dele é que o resultado do *Action* já é engregue imediatamente a nível de ActionFilter. A desvantagem é que o resultado fica "desatualizado" durante 3 minutos em relação a alguma modificação que possivelmente poderia acontecer em alguns dos registros.

Outra abordagem que poderia ter sido utilizada seria o Second Level Cache, que é um cache a nível de persistência. A vantagem dele é a possibilidade de invalidar uma entrada de cache quando um determinado registro é alterado. A desvantagem seria apenas o tempo de hydrate/dehydrate e serialize/deserialize.