using UnityEngine;
using System.Collections.Generic;

public class LeaderBehaviour : FlockingBaseBehaviour // se coloca en una entidad o waypoint
                                                     //se coloca en el controller de la entidad LeaderBehaviour _leaderBehaviour;
                                                     //Lo obtenemos en el start = _leaderBehaviour = GetComponent,LeaderBehaviour>();
                                                     //lo utilizamos en os estados con la FSM
                                                     //pasamos rigidbody al steering de la entidad(pasamos rigidbody por enica = RigidBody _target)
                                                     // lo gaurdamos en el cosntructor= _target = target;
                                                     //pasamos en el mismo constructor el LeaderBehaviour(pasamos leaderbehaviour por encima = LeaderBehaviour _leaderBehaviour)
                                                     //lo guardamos en el constructor = _leaderbehaviour = leaderBehaviour
                                                     //creamos un public override void Enter(){base.enter();- _leaderBehaviour.IsActive = true- _leaderBEhaviour.LederRB = _target;
                                                     //creamos un public override void Sleep()(exit){base.Sleep();_leaderBehaviour.IsActive=false;
                                                     //pasamos _leaderBehavior y target al controller
                                                     //llamar leaderBehavior con una variable local en la fsm del controller(GetComponent())
                                                     //Definimos target desde el instructor(ejemplo:jugador)
                                                     
{
    public float timePrediction;
    Seek _seek; //se le agrego un constructor y un transform a la clase seek
    Pursuit _pursuit; //se le agrego un constructor y un transform a la clase pursuit
    bool _isPursuit;

    private void Awake()
    {
        _pursuit = new Pursuit(transform, timePrediction);
        _seek = new Seek(transform);
    }
    protected override Vector3 GetRealDir(List<IBoid> boids, IBoid self)
    {
        if (_isPursuit)
        {
            return _pursuit.GetDir() * multiplier;
        }
        return _seek.GetDir() * multiplier;
    }
    public Transform Leader  //setearlo en enter o exit
    {
        set
        {
            var rb = value.GetComponent<Rigidbody>();

            if (rb)
            {
                _pursuit.Target = rb;
                _isPursuit = true;
            }
            else
            {
                _seek.Target = value;
                _isPursuit = false;
            }

        }
    }

    public Rigidbody LeaderRb  //setearlo en enter o exit
    {
        set
        {
              _pursuit.Target = value;
              _isPursuit = true;
          
        }
    }
}
