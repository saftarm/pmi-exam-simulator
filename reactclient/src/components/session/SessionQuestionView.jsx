import Icon from '../Icon';
import QuestionMap from '../QuestionMap';
import { SessionHeader } from '../SessionTimer';
import AppFooter from '../AppFooter';
import { LoadingButton } from '../loading';
import { normalizeQuestionType } from '../../utils/questionTypes';
import SessionOptionButton, { OPTION_LETTERS } from './SessionOptionButton';
import { isOptionSelected } from '../../utils/sessionAnswers';
import SessionToolsSidebar from './SessionToolsSidebar';

export default function SessionQuestionView({
  examTitle,
  durationMinutes,
  questions,
  currentIndex,
  answers,
  flagged,
  submitting,
  submitError,
  onSelectIndex,
  onSelectOption,
  onToggleFlag,
  onSubmit,
  onExpire,
}) {
  const total = questions.length;
  const currentQuestion = questions[currentIndex];
  const currentAnswer = answers[currentIndex];
  const options = currentQuestion?.answerOptions ?? [];
  const isMultipleChoice =
    normalizeQuestionType(currentQuestion?.questionType) === 'MultipleChoice';

  return (
    <div className="bg-background text-on-surface h-screen overflow-hidden flex flex-col">
      <SessionHeader
        durationMinutes={durationMinutes}
        currentIndex={currentIndex}
        totalQuestions={total}
        onSubmit={onSubmit}
        submitting={submitting}
        onExpire={onExpire}
      />

      <main className="flex flex-1 overflow-hidden">
        <QuestionMap
          total={total}
          currentIndex={currentIndex}
          answers={answers}
          flagged={flagged}
          onSelect={onSelectIndex}
        />

        <section className="flex-1 overflow-y-auto bg-background p-xl flex flex-col items-center custom-scrollbar">
          <div className="max-w-[800px] w-full space-y-lg fade-in-up" key={currentIndex}>
            <div className="flex items-center justify-between">
              <div>
                <span className="font-label-lg text-secondary uppercase tracking-widest">
                  Question {currentIndex + 1} of {total}
                </span>
                <h2 className="font-headline-lg text-headline-lg text-primary mt-xs">
                  {examTitle ?? 'Exam Session'}
                </h2>
              </div>
              <button
                type="button"
                onClick={onToggleFlag}
                className={`flex items-center gap-sm px-md py-sm rounded-full border font-label-lg transition-colors ${
                  flagged.has(currentIndex)
                    ? 'border-primary bg-primary-fixed text-primary'
                    : 'border-primary text-primary hover:bg-primary-fixed'
                }`}
              >
                <Icon name="flag" filled={flagged.has(currentIndex)} />
                Flag for Review
              </button>
            </div>

            <div className="bg-white border border-outline-variant rounded-xl p-xl shadow-sm">
              {isMultipleChoice && (
                <div className="mb-md p-md bg-primary-fixed/20 border border-primary/20 rounded-lg text-sm text-primary">
                  Select all answers that apply. Partial credit is awarded for each correct choice.
                </div>
              )}
              <p className="font-body-lg text-body-lg text-on-surface leading-relaxed">
                {currentQuestion.questionTitle}
              </p>
              <div className="mt-xl space-y-md">
                {options.map((option, i) => {
                  const letter = OPTION_LETTERS[i] ?? String(i + 1);
                  return (
                    <SessionOptionButton
                      key={option.id}
                      option={option}
                      letter={letter}
                      isSelected={isOptionSelected(currentAnswer, option.id)}
                      isMultipleChoice={isMultipleChoice}
                      onClick={() => onSelectOption(option.id)}
                    />
                  );
                })}
              </div>
            </div>

            {submitError && (
              <div className="p-md bg-red-50 border border-red-200 rounded-lg text-sm text-red-700">
                {submitError}
              </div>
            )}

            <div className="flex items-center justify-between pt-lg">
              <button
                type="button"
                onClick={() => onSelectIndex(Math.max(0, currentIndex - 1))}
                disabled={currentIndex === 0}
                className="flex items-center gap-sm px-xl py-md text-primary font-label-lg hover:bg-primary-fixed rounded-lg transition-colors disabled:opacity-40"
              >
                <Icon name="arrow_back" />
                Previous
              </button>
              <div className="flex items-center gap-md">
                <button
                  type="button"
                  onClick={onToggleFlag}
                  className="px-xl py-md bg-white border border-outline-variant text-on-surface-variant font-label-lg rounded-lg hover:bg-surface-container-lowest transition-all"
                >
                  Save for later
                </button>
                {currentIndex < total - 1 ? (
                  <button
                    type="button"
                    onClick={() => onSelectIndex(currentIndex + 1)}
                    className="px-xl py-md bg-secondary-container text-on-secondary-container font-label-lg rounded-lg hover:brightness-110 shadow-lg flex items-center gap-sm transition-all active:scale-95"
                  >
                    Next Question
                    <Icon name="arrow_forward" />
                  </button>
                ) : (
                  <LoadingButton
                    onClick={onSubmit}
                    loading={submitting}
                    loadingText="Submitting…"
                    className="px-xl py-md bg-secondary-container text-on-secondary-container font-label-lg rounded-lg hover:brightness-110 shadow-lg transition-all active:scale-95 disabled:opacity-60"
                  >
                    Submit Exam
                    {!submitting && <Icon name="check" />}
                  </LoadingButton>
                )}
              </div>
            </div>
          </div>
        </section>

        <SessionToolsSidebar answers={answers} total={total} flaggedCount={flagged.size} />
      </main>

      <AppFooter compact />
    </div>
  );
}
