﻿using System.Collections;

public interface IEffect
{
    IEnumerator Apply(Character user, Character target);
}
