import Icon from '../../../../shared/components/Icon';
import type { AnswerOptionDto } from '../../../admin-questions/types';

interface SessionOptionButtonProps {
  option: AnswerOptionDto;
  letter: string;
  isSelected: boolean;
  isMultipleChoice: boolean;
  onClick: () => void;
}

export default function SessionOptionButton({
  option,
  letter,
  isSelected,
  isMultipleChoice,
  onClick,
}: SessionOptionButtonProps) {
  return (
    <button
      type="button"
      onClick={onClick}
      className={`group cursor-pointer flex items-start gap-md p-md w-full text-left rounded-xl transition-all ${
        isSelected
          ? 'border-2 border-primary bg-primary-fixed/30'
          : 'border border-outline-variant hover:border-primary-fixed hover:bg-surface-container-lowest'
      }`}
    >
      <div
        className={`mt-1 flex-shrink-0 h-6 w-6 border-2 flex items-center justify-center font-label-lg ${
          isMultipleChoice ? 'rounded' : 'rounded-full'
        } ${
          isSelected
            ? 'border-primary bg-primary text-white'
            : 'border-outline-variant text-on-surface-variant group-hover:border-primary-fixed'
        }`}
      >
        {isMultipleChoice && isSelected ? (
          <Icon name="check" style={{ fontSize: 14 }} />
        ) : (
          letter
        )}
      </div>
      <p className="font-body-md text-body-md text-on-surface">{option.text}</p>
    </button>
  );
}

export const OPTION_LETTERS = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
