/** Defines an object that can be faded in and out. */
public interface IFadable
{
    /** Fades this object in. Returns the approximate it will take to do so. */
    public void FadeIn();

    /** Fades this object out. Returns the approximate it will take to do so. */
    public void FadeOut();

    /** Makes the object instantly visible */
    public void CutIn();

    /** Makes the object instantly invisible */
    public void CutOut();
}