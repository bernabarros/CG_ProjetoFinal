# Relatório Trabalho Final de Computação Gráfica
### Realizado por: Bernardo Barros a22401588 Simão Durão a22408594

## Introdução

Neste relatório descrevemos o trabalho realizado no âmbito da cadeira de Computação Gráfica no intiuto de implementar as técnicas de Ambient Occlusion e Screen Space Shadows.

Ambient Occlusion é uma técnica em que certas zonas do mundo que, por exemplo cantos, são escurecidas devido à menor incidência de luz. É obtida através do lançamento de raios de amostra que ao embaterem com geometria próxima iram indicar o certo de oclusão que depois é aplicado à superfície.

Por sua vez, Screen Space Shadows é uma técnica de funcionamento semelhante ao Ambient Occlusion, porém aqui é tida em conta um ponto de luz direto e não um valor de luz ambiente.

Estas técnicas serão implementadas em ambiente OpenGL, mais precisamente OpenTK que utiliza a linguagem C# dada no decorrer do curso.

---
## Implementação

Para que seja mais simples a implementação destas técnicas de computação gráfica, foi usado como base um programa de OpenTK desenvolvido pelos professores Diogo de Andrade e Nuno Fachada em que é gerada um janela e desenhada um floresta utilizando meshes (plano para o chãos e cilindros para as árvores). O nosso objetivo seria implementar as técnicas de modo a que certas zonas sejam escurecidas através AO, por exemplo as zonas em que as árvores interceptam com o chão e ao aplicar uma luz direta, possamos observar sombras como as quais obtidas através de Screen Space Shadows.

Numa primeira tentativa, experimentamos implementar o AO apenas fazendo modificações simples aos shaders, com alterações mínimas ao código principal.
Adicionamos um valor de luz ambiente e no shaders, esse valor será afetado por um fator de oclusão. Esse fator é aplicado se uma superfície não esteja com a sua normal com direção para o céu.
O resultado foi que as superfícies com direção que não seja (0,1,0) foram escurecidas, enquanto que o resto mantiveram o seu valor de luminosidade.

Obviamente o resultado desta experiência não resultou em AO mas deu para entender de que se criamos implementar AO teriamos de ir mais longe e implementar vários componentes para obter o efeito desejado.

O programa no seu estado inicial não implementa um G-Buffer por isso tivemos de adicionar uma classe para a implementação do mesmo, bem como um novo render pipeline que aplicaria o efeito de AO, mais precisamente de Screen Space Ambient Occlusion. Foram também criados shaders de geometria para trabalhar com o G-Buffer.
Antes de prosseguirmos testamos o que foi adicionado até agora para verificar o seu funcionamento, em que fizemos com que as normais sejam dadas ao G-Buffer e através dessa informação é desenhado um triângulo.
Porém o teste revelou falhas na implementação, apenas sendo possível obter um ecrã preto ou ecrã cinzento e apesar de várias tentativas não foi possível resolver a questão.

Idealmente, se a questão tivesse sido resolvida, iriamos passar para implementação do kernel que faria as amostras para verificar o fator de oclusão de cada pixel e através dessa informação aplicar ao fator da luz ambiente. Seria também usado um blur para melhorar o aspeto gráfico da cena gerada.


## Conclusão

## Referências