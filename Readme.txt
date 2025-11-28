mon synth n'est qu'une classe qui lis les paternes, set les valeurs de l'onde et créé des notes

une note est un objet volatile qui est créé puis vie sa durée puis ce supprime une fois fini

Enveloppe ADSR

A : Attack
D : Decay
S : Sustain
R : Release

type de courbe 

Linéaire
Exponentielle
Logarithmique

choix de la courbe avec une callback



note qui prend une struct ADSR, celle si prend les temp et le taux Sustain ainsi que les courbes et ma state



[Linéaire 0 ]