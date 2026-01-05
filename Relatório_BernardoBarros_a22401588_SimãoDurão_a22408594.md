# Relatório Trabalho Final de Computação Gráfica
### Realizado por: Bernardo Barros a22401588 Simão Durão a22408594

## Introdução

Neste relatório descrevemos o trabalho realizado no âmbito da cadeira de Computação Gráfica no intiuto de implementar as técnicas de Ambient Occlusion e Screen Space Shadows.

Ambient Occlusion é uma técnica em que certas zonas do mundo que, por exemplo cantos, são escurecidas devido à menor incidência de luz. É obtida através do lançamento de raios de amostra que ao embaterem com geometria próxima iram indicar o certo de oclusão que depois é aplicado à superfície.

![SSAO](image.png)

Por sua vez, Screen Space Shadows (Contact Shadows) é uma técnica de funcionamento semelhante ao Ambient Occlusion, porém aqui é tida em conta um ponto de luz direto e não um valor de luz ambiente.
![alt text](image-1.png)

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

![alt text](image-7.png)

Esta imagem é a representação de um debug normal Shader utilizado para SSAO.

Idealmente, se a questão tivesse sido resolvida, iriamos passar para implementação do kernel que faria as amostras para verificar o fator de oclusão de cada pixel e através dessa informação aplicar ao fator da luz ambiente. Seria também usado um blur para melhorar o aspeto gráfico da cena gerada.

Para a implementação da técnica de Screen Space Shadows (SSS), partimos com o objetivo de simular o bloqueio da luz direta pelas árvores. Queríamos garantir que ao projetar uma luz direcional na floresta, as árvores projetassem umas sombras.

Na nossa primeira tentativa, Inspiramo-nos a implementação de SSS de  [Panos Karabelas](https://panoskarabelas.com/posts/screen_space_shadows/) e tentamos modificar os shaders existentes para ver o que se poderia assimilar com uma sombra, mas como não havia um shader de luz, ainda não se poderia ver o efeito das sombras. 

Na segunda, tentamos implementar os shader lightmatrix_sss, depthmap_sss, fragshader_sss e a Render Pipeline (RPS_SSS). Esses shaders fazem todos parte da técnica SSS, eles são recursos fundamentais para tal, se não funcionarem, pode haver SSS. Ao testar foi impossível validar o funcionamento dela, o  programa acabou fechando sozinho.

![alt text](image-6.png)

Esta imagem seria a representação de um Depth Map para SSS.

Depois de tentativa e erro, para resolver o Render Pipeline e conseguir o por a funcionar corretamente para verificar o funcionamento dos shaders. Houve momentos em que o ecrã ficava só com a cor preta e indicava erros na consola. Depois de alguns segundos acabava por fechar.

Encontra-mos também exemplos de SSAO e SSS, 

### Screen Space Shadows

![alt text](image-2.png) 
![alt text](image-3.png)

### Screen Space Ambient Occlusion

![alt text](image-4.png)
![alt text](image-5.png)

## Conclusão

Concluindo, embora as nossas pesquisas sobre os temas selecionados nos tenha dado uma ideia de como funciona tanto a técnica de Ambient Occlusion e Screen Space Shadows, sendo estes tópicos avançados de Computação Gráfica, devidos as complicações na nossa estrutura do G-Buffer e da render pipeline, não foi possível obter uma implementação total destas mesmas.

## Referências

[Epic Games explain Contact Shadows in Unreal Engine](https://dev.epicgames.com/documentation/en-us/unreal-engine/contact-shadows-in-unreal-engine)

[https://learnopengl.com/Advanced-Lighting/SSAO](https://learnopengl.com/Advanced-Lighting/SSAO)

[https://github.com/Baksonator/ssao](https://github.com/Baksonator/ssao)

[https://gist.github.com/transitive-bullshit/6770363](https://gist.github.com/transitive-bullshit/6770363)

[https://github.com/lettier/3d-game-shaders-for-beginners/blob/master/sections/ssao.md](https://github.com/lettier/3d-game-shaders-for-beginners/blob/master/sections/ssao.md)

[Image of SSAO](https://bobby-parker.com/architectural-rendering-blog/all-you-need-to-know-about-ambient-occlusion)

[Image of SSAO](https://www.sciencedirect.com/topics/computer-science/ambient-occlusion)

[Preview de SSS](https://www.shadertoy.com/view/mtXfDf)

[Articulo sobre SSS](https://panoskarabelas.com/posts/screen_space_shadows/)

[Shadow Mapping](https://learnopengl.com/Advanced-Lighting/Shadows/Shadow-Mapping)

[Point Shadows](https://learnopengl.com/Advanced-Lighting/Shadows/Point-Shadows)

[Lighting](https://learnopengl.com/Lighting/Basic-Lighting)

[Depth](https://learnopengl.com/Advanced-OpenGL/Depth-testing)

[Github de um SSS em OpenTK](https://github.com/egnawake/screen-space-shadows)

[Comparison of SSS](https://blenderartists.org/t/how-to-disable-contact-shadows-for-object/1403230)
